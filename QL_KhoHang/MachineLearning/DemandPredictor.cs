using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ML.Data;
using QL_KhoHang.MachineLearning;
namespace QL_KhoHang.MachineLearning
{
    public class DemandPredictor
    {
        private readonly MLContext mlContext;
        private ITransformer model;

        public DemandPredictor()
        {
            mlContext = new MLContext();
        }

        public void TrainModel(IEnumerable<PhieuXuatData> data)
        {
            var dataView = mlContext.Data.LoadFromEnumerable(data);

            // Xây dựng pipeline
            var pipeline = mlContext.Transforms.Concatenate("Features", "Thang", "Nam")
                .Append(mlContext.Regression.Trainers.Sdca(labelColumnName: "SoLuong"));

            // Huấn luyện mô hình
            model = pipeline.Fit(dataView);
        }

        public float Predict(int thang, int nam, string sanPhamID)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<DemandPrediction, DemandPredictionResult>(model);

            var input = new DemandPrediction { Thang = thang, Nam = nam, SanPhamID = sanPhamID };
            var result = predictionFunction.Predict(input);

            return result.SoLuong;
        }
    }
}
