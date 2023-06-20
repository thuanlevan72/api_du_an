namespace FOLYFOOD.Entitys
{
    public class Voucher
    {
        public int voucherId { get; set; }
        public string VoucherName { get; set; }
        public string VoucherCode { get; set; }
        public string Valuevoucher { get; set; }
        public DateTime expirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
