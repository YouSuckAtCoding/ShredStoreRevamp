CREATE PROCEDURE [dbo].[spUsuario_Login]
	@Nome nvarchar(50),
	@Password nvarchar(50),
	@ResponseMessage nvarchar(250)='' Output

AS
Begin
	
	SET NOCOUNT ON

	Declare @SelectedId int

	IF Exists (Select Top 1 Id From dbo.Usuario Where Nome = @Nome)
	Begin
		Set @SelectedId=(Select Id from dbo.Usuario Where Nome = @Nome And Password=HASHBYTES('SHA2_512',@Password+Cast(Salt As nvarchar(36))))

		If(@SelectedId Is Null)
			Set @ResponseMessage='Incorret Password'
		Else
			Set @ResponseMessage='Login Successfull'
			exec dbo.spUsuario_GetById @SelectedId
			
	End
	Else
		Set @ResponseMessage='Invalid Login Attempt'

End
	