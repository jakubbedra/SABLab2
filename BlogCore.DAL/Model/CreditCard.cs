namespace Blog.DAL.Model;

public class CreditCard
{
    public long Number {get; set; }
    public string ExpirationDate { get; set; }
    public int SecurityCode { get; set; }
}