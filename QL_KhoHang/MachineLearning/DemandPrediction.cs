using Microsoft.ML.Data;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ML.Data;

namespace QL_KhoHang.MachineLearning
{
    // Lớp đầu vào cho dự đoán
    public class DemandPrediction
    {
        [LoadColumn(0)]
        public float Thang { get; set; }

        [LoadColumn(1)]
        public float Nam { get; set; }

        [LoadColumn(2)]
        public string SanPhamID { get; set; }
    }

    // Lớp kết quả dự đoán
    public class DemandPredictionResult
    {
        [ColumnName("Score")]
        public float SoLuong { get; set; }
    }
}
