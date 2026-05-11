using SaigonBus.Controllers;
using SaigonBus.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SaigonBus.Services
{
    public static class PointService
    {


        /// <summary>Cứ mỗi 1.000 VNĐ thanh toán → 1 điểm.</summary>
        private const decimal VndPerPoint = 1000m;

        /// <summary>Thưởng thêm khi trả xe về trạm đang thiếu xe (discount > 0).</summary>
        private const int BonusReturnLowStation = 50;

        /// <summary>Thưởng thêm khi chọn xe đạp thường (thân thiện môi trường).</summary>
        private const int BonusStandardBike = 5;

        /// <summary>Thưởng thêm nếu chuyến đi xảy ra trong giờ cao điểm.</summary>
        private const int BonusPeakHour = 5;


        /// <summary>
        /// Tính điểm thưởng cho một chuyến đi đơn lẻ (SaveTrip / ReturnGroupRental).
        /// </summary>
        /// <param name="finalAmount">Số tiền thực tế khách trả (đã trừ discount).</param>
        /// <param name="discountAmount">Số tiền được giảm giá (0 nếu không có).</param>
        /// <param name="vehicleCategory">Loại xe (StandardBike / EBike / Scooter).</param>
        /// <param name="tripStartTime">Thời điểm bắt đầu chuyến đi, để kiểm tra giờ cao điểm.</param>
        /// <param name="quantity">Số xe (mặc định 1, truyền > 1 cho thuê nhóm).</param>
        /// <returns>Tổng điểm được cộng và breakdown để log.</returns>
        public static PointResult Calculate(
            decimal finalAmount,
            decimal discountAmount,
            string vehicleCategory,
            DateTime tripStartTime,
            int quantity = 1)
        {
            var result = new PointResult();

            result.BasePoints = (int)Math.Ceiling(finalAmount / VndPerPoint);

            if (discountAmount > 0)
            {
                result.BonusLowStation = BonusReturnLowStation * quantity;
                result.Reasons += $"Trả xe trạm thiếu (+{result.BonusLowStation}đ); ";
            }

            if (vehicleCategory == "StandardBike" || vehicleCategory == VehicleCategory.StandardBike.ToString())
            {
                result.BonusEco = BonusStandardBike * quantity;
                result.Reasons += $"Xe đạp xanh (+{result.BonusEco}đ); ";
            }

            int hour = tripStartTime.Hour;
            bool isPeakHour = (hour >= 7 && hour < 9) || (hour >= 17 && hour < 19);
            if (isPeakHour)
            {
                result.BonusPeak = BonusPeakHour * quantity;
                result.Reasons += $"Giờ cao điểm (+{result.BonusPeak}đ); ";
            }

            result.TotalPoints = result.BasePoints + result.BonusLowStation + result.BonusEco + result.BonusPeak;
            return result;
        }

        public static void ApplyPoints(User user, PointResult result, SaigonRideContext db)
        {
            if (result.TotalPoints <= 0) return;

            user.Point += result.TotalPoints;

            try
            {
                var log = new PointTransaction
                {
                    UserId = user.UserId,
                    Points = result.TotalPoints,
                    Reason = result.Reasons.TrimEnd(' ', ';'),
                    CreatedAt = DateTime.Now
                };
                db.PointTransactions.Add(log);
            }
            catch
            {
            }
        }
    }

    public class PointResult
    {
        public int BasePoints { get; set; }
        public int BonusLowStation { get; set; }
        public int BonusEco { get; set; }
        public int BonusPeak { get; set; }
        public int TotalPoints { get; set; }
        public string Reasons { get; set; } = "";
    }
}