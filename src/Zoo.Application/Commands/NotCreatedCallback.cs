namespace Zoo.Application.Commands
{
    public delegate void NotCreatedCallback<in TCreating>(TCreating creating);
}