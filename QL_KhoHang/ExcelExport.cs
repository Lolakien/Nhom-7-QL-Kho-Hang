﻿using System;
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
            string ngay = "Ngày " + pn.NgayNhap. Day + " Tháng " + pn.NgayNhap.Month + " Năm " + pn.NgayNhap.Year;
            replacer.Add("%NgayThangNam", ngay);
            QL_KhoHangDataContext qlhh = new QL_KhoHangDataContext();
            NhaCungCap ncc = qlhh.NhaCungCaps.Where(t => t.NhaCungCapID ==
            pn.NhaCungCapID).FirstOrDefault();
            replacer.Add("%MaPN", pn.PhieuNhapID);
            replacer.Add("%NCC", ncc.TenNhaCungCap);
            string diachi = ncc.DiaChi + " " + ncc.ThanhPho;
            replacer.Add("%DiaChi", diachi);
            replacer.Add("%DienThoai", ncc.DienThoai);
            replacer.Add("%TongTien", String.Format("{0:0,0.00} ", pn.TongTien));
            NhanVien nv = qlhh. NhanViens.Where(t => t.NhanVienID == pn.NhanVienID).FirstOrDefault();
            replacer.Add("%TenNV", nv.HoTen);
            MemoryStream stream = null;
            byte[] arrByte = new byte[0];
            arrByte = File.ReadAllBytes("phieunhaphang.xlsx").ToArray();
            //Get stream
            if (arrByte.Count() > 0)
            {
                stream = new MemoryStream(arrByte);
            }
            ExcelEngine engine = new ExcelEngine();
            IWorkbook workBook = engine.Excel.Workbooks.Open(stream);
            IWorksheet workSheet = workBook.Worksheets[0];
            ITemplateMarkersProcessor markProcessor = workSheet.CreateTemplateMarkersProcessor();
            //Replace value
            if (replacer != null && replacer.Count > 0)
            {
                //Find and replace values
                foreach (KeyValuePair<string, string> repl in replacer)
                {
                    Replace (workSheet, repl.Key, repl.Value);
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

            // Kiểm tra dữ liệu
            if (cthdSTT.Count > 0)
            {
                string viewName = "Phieunhaphang";
                markProcessor.AddVariable(viewName, cthdSTT);
                markProcessor.ApplyMarkers(UnknownVariableAction.ReplaceBlank);
            }
            else
            {
                MessageBox.Show("Không có chi tiết phiếu nhập để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            //List<ChiTietPhieuNhap> ctpns = pn.ChiTietPhieuNhaps.Where(t => t.PhieuNhapID == pn.PhieuNhapID).ToList();
            //List<ChiTietPN> cthdSTT = new List<ChiTietPN>();
            //int stt = 1;
            //foreach (ChiTietPhieuNhap ct in ctpns)
            //{
            //    ChiTietPN ctstt = new ChiTietPN(ct, stt);
            //    ctstt.TenSP = qlhh.SanPhams.Where(t => t.SanPhamID == ct.SanPhamID).FirstOrDefault().TenSanPham;
            //    cthdSTT.Add(ctstt);
            //}

            //string viewName = "Phieunhaphang";
            //markProcessor.AddVariable(viewName, cthdSTT);
            //markProcessor.ApplyMarkers(UnknownVariableAction.ReplaceBlank);


            //Delete temporary row
            Syncfusion.XlsIO.IRange range = workSheet.FindFirst("[TMP]", ExcelFindType.Text);
            if (range != null)
            {
                workSheet.DeleteRow(range.Row);
            }
            string file = Path.GetTempFileName() + Constants.FILE_EXT_XLS;
            fileName = file;
            //Output file
            if (!FileCommon.IsFileOpenOrReadOnly(file))
            {
            workBook.SaveAs (file);
            }
            //Close
            workBook.Close();
            engine.Dispose();
            if (!string.IsNullOrEmpty(fileName) && MessageBox.Show("Bạn có muốn mở file không?", "Thông tin", MessageBoxButtons. YesNo, MessageBoxIcon. Information, MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(fileName);
            }



         }
    }
}
