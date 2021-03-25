namespace Zoo.Application.Commands
{
    public delegate void CreatedCallback<in TDetails>(TDetails details);
}