using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using Syncfusion.XlsIO;
using System.Windows.Forms;

namespace QL_KhoHang
{
    class ExcelExport
    {
        private Excel.Application _excelApp;
        private Excel.Workbook _workbook;
        private Excel.Worksheet _worksheet;


        public ExcelExport()
        {
            _excelApp = new Excel.Application();
            _excelApp.Visible = false; // Không hiển thị ứng dụng Excel
            _workbook = _excelApp.Workbooks.Add();
            _worksheet = (Excel.Worksheet)_workbook.Sheets[1];
        }
        public ExcelExport(string filePath)
        {
            _excelApp = new Excel.Application();
            _excelApp.Visible = false; // Không hiển thị ứng dụng Excel
            _workbook = _excelApp.Workbooks.Add();
            _worksheet = (Excel.Worksheet)_workbook.Sheets[1];
            _workbook.SaveAs(filePath);
        }
        private static void Replace(IWorksheet workSheet, string findValue, string replaceValue)
        {
            // Find and replace
            if (workSheet != null && !string.IsNullOrEmpty(findValue))
            {
                // Get current cells
                Syncfusion.XlsIO.IRange[] cells = workSheet.Range.Cells;
                Syncfusion.XlsIO.IRange range = null;

                // Loop cells to replace
                for (int i = 0; i < cells.Count(); i++)
                {
                    // Current cell
                    range = cells[i];

                    // Find and replace values
                    if (range != null && range.DisplayText.Contains(findValue))
                    {
                        range.Text = range.Text.Replace(findValue, replaceValue);
                        break;
                    }
                }
            }
        }
        public void ExportPhieuNhap(PhieuNhap pn, ref string fileName, bool isPrintPreview)
        {
            Dictionary<string, string> replacer = new Dictionary<string, string>();
            string ngay = "Ngày " + pn.NgayNhap.Day + " Tháng " + pn.NgayNhap.Month + " Năm " + pn.NgayNhap.Year;
            replacer.Add("%NgayThangNam", ngay);

            QL_KhoHangDataContext qlhh = new QL_KhoHangDataContext();
            NhaCungCap ncc = qlhh.NhaCungCaps.FirstOrDefault(t => t.NhaCungCapID == pn.NhaCungCapID);
            replacer.Add("%MaPN", pn.PhieuNhapID);
            replacer.Add("%NCC", ncc.TenNhaCungCap);

            string diachi = ncc.DiaChi + " " + ncc.ThanhPho;
            replacer.Add("%DiaChi", diachi);
            replacer.Add("%DienThoai", ncc.DienThoai);
            replacer.Add("%TongTien", String.Format("{0:0,0.00} ", pn.TongTien));

            NhanVien nv = qlhh.NhanViens.FirstOrDefault(t => t.NhanVienID == pn.NhanVienID);
            replacer.Add("%TenNV", nv.HoTen);

            MemoryStream stream = null;
            byte[] arrByte = File.ReadAllBytes("phieunhaphang.xlsx").ToArray();

            // Get stream
            if (arrByte.Length > 0)
            {
                stream = new MemoryStream(arrByte);
            }

            ExcelEngine engine = new ExcelEngine();
            IWorkbook workBook = engine.Excel.Workbooks.Open(stream);
            IWorksheet workSheet = workBook.Worksheets[0];
            ITemplateMarkersProcessor markProcessor = workSheet.CreateTemplateMarkersProcessor();

            // Replace value
            if (replacer != null && replacer.Count > 0)
            {
                foreach (KeyValuePair<string, string> repl in replacer)
                {
                    Replace(workSheet, repl.Key, repl.Value);
                }
            }

            // Tạo danh sách ChiTietPN từ ChiTietPhieuNhap
            List<ChiTietPhieuNhap> ctpns = pn.ChiTietPhieuNhaps.Where(t => t.PhieuNhapID == pn.PhieuNhapID).ToList();
            List<ChiTietPN> cthdSTT = new List<ChiTietPN>();
            int stt = 1;

            foreach (ChiTietPhieuNhap ct in ctpns)
            {
                ChiTietPN ctstt = new ChiTietPN(ct, stt);
                cthdSTT.Add(ctstt);
                stt++;
            }

            // Tìm dòng có chuỗi [TMP]
            Syncfusion.XlsIO.IRange tmpRange = workSheet.FindFirst("[TMP]", ExcelFindType.Text);
            if (tmpRange != null)
            {
                int rowIndex = tmpRange.Row;

                // Thêm dòng trống tương ứng với số lượng chi tiết phiếu nhập
                if (cthdSTT.Count > 0)
                {
                    for (int i = 0; i < cthdSTT.Count; i++)
                    {
                        workSheet.InsertRow(rowIndex); // Chèn dòng mới
                    }

                    // Thêm biến vào marker
                    string viewName = "Phieunhaphang";
                    markProcessor.AddVariable(viewName, cthdSTT);
                    markProcessor.ApplyMarkers(UnknownVariableAction.ReplaceBlank);
                }
                else
                {
                    MessageBox.Show("Không có chi tiết phiếu nhập để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            string file = Path.GetTempFileName() + Constants.FILE_EXT_XLS;
            fileName = file;
            // Delete temporary row
            Syncfusion.XlsIO.IRange range = workSheet.FindFirst("[TMP]", ExcelFindType.Text);
            if (range != null)
            {
                workSheet.DeleteRow(range.Row);
            }

            // Output file
            if (!FileCommon.IsFileOpenOrReadOnly(file))
            {
                workBook.SaveAs(file);
            }

            // Close
            workBook.Close();
            engine.Dispose();

            if (!string.IsNullOrEmpty(fileName) && MessageBox.Show("Bạn có muốn mở file không?", "Thông tin", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(fileName);
            }
        }

        public void ExportPhieuXuat(PhieuXuat px, ref string fileName, bool isPrintPreview)
        {
            Dictionary<string, string> replacer = new Dictionary<string, string>();
            string ngay = "Ngày " + px.NgayXuat.Day + " Tháng " + px.NgayXuat.Month + " Năm " + px.NgayXuat.Year;
            replacer.Add("%NgayThangNam", ngay);

            QL_KhoHangDataContext qlhh = new QL_KhoHangDataContext();
            KhachHang kh = qlhh.KhachHangs.FirstOrDefault(t => t.KhachHangID == px.KhachHangID);
            replacer.Add("%MaPX", px.PhieuXuatID);
            replacer.Add("%KH", kh.TenKH);

            string diachi = kh.DiaChi + " " + kh.ThanhPho;
            replacer.Add("%DiaChi", diachi);
            replacer.Add("%DienThoai", kh.SDT);
            replacer.Add("%TongTien", String.Format("{0:0,0.00}", px.TongTien));

            NhanVien nv = qlhh.NhanViens.FirstOrDefault(t => t.NhanVienID == px.NhanVienID);
            replacer.Add("%TenNV", nv.HoTen);

            MemoryStream stream = null;
            byte[] arrByte = File.ReadAllBytes("phieuxuathang.xlsx").ToArray();

            // Get stream
            if (arrByte.Length > 0)
            {
                stream = new MemoryStream(arrByte);
            }

            ExcelEngine engine = new ExcelEngine();
            IWorkbook workBook = engine.Excel.Workbooks.Open(stream);
            IWorksheet workSheet = workBook.Worksheets[0];
            ITemplateMarkersProcessor markProcessor = workSheet.CreateTemplateMarkersProcessor();

            // Replace value
            if (replacer != null && replacer.Count > 0)
            {
                foreach (KeyValuePair<string, string> repl in replacer)
                {
                    Replace(workSheet, repl.Key, repl.Value);
                }
            }

            // Create a list of ChiTietPX from ChiTietPhieuXuat
            List<ChiTietPhieuXuat> ctpxs = px.ChiTietPhieuXuats.Where(t => t.PhieuXuatID == px.PhieuXuatID).ToList();
            List<ChiTietPX> ctpxSTT = new List<ChiTietPX>();
            int stt = 1;

            foreach (ChiTietPhieuXuat ct in ctpxs)
            {
                ChiTietPX ctstt = new ChiTietPX(ct, stt);
                ctpxSTT.Add(ctstt);
                stt++;
            }

            // Tìm dòng có chuỗi [TMP]
            Syncfusion.XlsIO.IRange tmpRange = workSheet.FindFirst("[TMP]", ExcelFindType.Text);
            if (tmpRange != null)
            {
                int rowIndex = tmpRange.Row;

                // Thêm dòng trống tương ứng với số lượng chi tiết phiếu xuất
                if (ctpxSTT.Count > 0)
                {
                    for (int i = 0; i < ctpxSTT.Count; i++)
                    {
                        workSheet.InsertRow(rowIndex); // Chèn dòng mới
                    }

                    // Thêm biến vào marker
                    string viewName = "Phieuxuathang";
                    markProcessor.AddVariable(viewName, ctpxSTT);
                    markProcessor.ApplyMarkers(UnknownVariableAction.ReplaceBlank);
                }
                else
                {
                    MessageBox.Show("Không có chi tiết phiếu xuất để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            string file = Path.GetTempFileName() + Constants.FILE_EXT_XLS;
            fileName = file;
            // Delete temporary row
            Syncfusion.XlsIO.IRange range = workSheet.FindFirst("[TMP]", ExcelFindType.Text);
            if (range != null)
            {
                workSheet.DeleteRow(range.Row);
            }

            // Output file
            if (!FileCommon.IsFileOpenOrReadOnly(file))
            {
                workBook.SaveAs(file);
            }

            // Close
            workBook.Close();
            engine.Dispose();

            if (!string.IsNullOrEmpty(fileName) && MessageBox.Show("Bạn có muốn mở file không?", "Thông tin", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(fileName);
            }
        }


    }
}

