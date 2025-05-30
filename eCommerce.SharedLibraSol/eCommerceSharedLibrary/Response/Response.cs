namespace eCommerceSharedLibrary.Responses
{
    // Tạp kết quả phản hồi gồm cờ (Flag) và thông báo (Mesage)
    public record Response(bool Flag = false, string Message = null!);
    
}
