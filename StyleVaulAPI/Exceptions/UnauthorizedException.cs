namespace StyleVaulAPI.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string mensagem)
            : base(mensagem) { }
    }
}
