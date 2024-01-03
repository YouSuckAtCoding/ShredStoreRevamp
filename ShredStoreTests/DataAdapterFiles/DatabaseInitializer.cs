using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShredStoreTests.DataAdapterFiles
{
    public class DatabaseInitializer
    {
        private ISqlAccessConnectionFactory _connectionFactory;

        public DatabaseInitializer(ISqlAccessConnectionFactory sqlAccessConnectionFactory)
        {

            _connectionFactory = sqlAccessConnectionFactory;
        }

        public async Task InitializeAsync()
        {
            using var connection = await _connectionFactory.CreateConnectionAsync();
            
                await connection.ExecuteAsync(
                @"CREATE TABLE Usuario (
                    Id INT NOT NULL PRIMARY KEY IDENTITY,
	                Nome VARCHAR(20) NOT NULL,
	                Idade INT NOT NULL,
	                Email varchar(50) NOT NULL,
	                Cpf VARCHAR(11) NOT NULL,
	                Endereco VARCHAR(50) NOT NULL,
	                Password BINARY(64) NOT NULL, 
                    Salt UNIQUEIDENTIFIER NOT NULL
                );"
                );
                await CreateUsuarioStoredProcedures(connection);

            
            
        }

        private async Task CreateUsuarioStoredProcedures(IDbConnection connection)
        {
            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUsuario_Insert]
            	@NOME VARCHAR(20),
            	@IDADE INT,
            	@EMAIL VARCHAR(50),
            	@CPF VARCHAR(11),
            	@ENDERECO VARCHAR(50),
            	@Password nvarchar(50)
            
                AS
                BEGIN
            	SET NOCOUNT ON
            	DECLARE @Salt UNIQUEIDENTIFIER=NEWID()
            
            	INSERT INTO dbo.Usuario (Nome, Idade, Email, Cpf, Endereco, [Password], Salt)
            	VALUES (@NOME, @IDADE, @EMAIL, @CPF, @ENDERECO,HASHBYTES('SHA2_512', @Password+CAST(@Salt as NVARCHAR(36))), @Salt)
                END"
                );

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE[dbo].[spUsuario_GetAll]
                AS
                Begin
                    Select Id, Nome, Idade, Email, Cpf, Endereco from dbo.Usuario
                End
                ");

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUsuario_GetById]
                @Id int
                AS
                Begin
                	Select Id, Nome, Idade, Email, Cpf, Endereco from dbo.Usuario
                	Where Id = @Id
                End");

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUsuario_Delete]
                @Id int
                AS
                Begin
                	Delete from dbo.Usuario where Id = @Id
                End	");

            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUsuario_Update]
            	@Id int,
            	@Nome VARCHAR(20),
            	@Idade INT,
            	@Email VARCHAR(50),
            	@Cpf VARCHAR(11),
            	@Endereco VARCHAR(50)
                AS
                Begin
            
            	Update dbo.Usuario 
            	Set Nome = @Nome, 
            	Idade = @Idade,
            	Email = @Email,
            	Cpf = @Cpf,
            	Endereco = @Endereco
            	Where Id = @Id
            
                End");
            await connection.ExecuteAsync(
                @"CREATE PROCEDURE [dbo].[spUsuario_Login]
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
            
                End");
        }
    }
}
