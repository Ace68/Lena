using Lena.Enumerations;

namespace Lena.Tests.Core;

public class PaymentResult(int id, string code, string name) : Enumeration(id, code, name)
{
    public static PaymentResult PaymentSucceded = new(0, "0", nameof(PaymentSucceded) );
    public static PaymentResult PaymentFailed = new(1, "1", nameof(PaymentFailed));
    public static PaymentResult PaymentPending = new(2, "2", nameof(PaymentPending));
    
    private static IEnumerable<PaymentResult> List() =>
    [
        PaymentSucceded,
        PaymentFailed,
        PaymentPending
    ];
    
    public static PaymentResult FromName(string name)
    {
        var paymentResult = List().SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

        return paymentResult ??
               throw new Exception($"Possible values for PaymentResult: {string.Join(",", List().Select(s => s.Name))}");
    }

    public static PaymentResult From(int id)
    {
        var paymentResult = List().SingleOrDefault(s => s.Id == id);

        return paymentResult ??
               throw new Exception($"Possible values for PaymentResult: {string.Join(",", List().Select(s => s.Name))}");
    }
}