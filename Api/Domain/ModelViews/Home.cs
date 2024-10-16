namespace asp_minimals_apis.Domain.ModelViews
{
    public struct Home
    {
        public string Message { get => "Welcome to Vehicle Minimal API."; }
        public string Docs
        { get => "/swagger"; }
    }
}