-- ======================================================================
-- Name: [adm].[uspAddUser]
-- Desc: Se utiliza para la creación de nuevos usuarios
-- Auth: Jonathan Piedra jonitapc_quimind@hotmail.com
-- Date: 5/24/2019
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspAddUser]
	@InsertUser		VARCHAR(50),
	@FullName		VARCHAR(50),
	@UserName		VARCHAR(50),
	@Email			VARCHAR(50),
	@Password		VARCHAR(50),
	@RoleID			INT
AS 
    BEGIN
        SET NOCOUNT ON
        SET XACT_ABORT ON
                           
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT
            DECLARE @lLocalTran BIT = 0
                               
            IF @@TRANCOUNT = 0 
                BEGIN
                    BEGIN TRANSACTION
                    SET @lLocalTran = 1
                END

            -- =======================================================
				INSERT INTO [adm].[utbUsers] ([RoleID],[FullName],[UserName],[Email],[PasswordHash],[LastActivityDate],[CreationUser],[LastModifyUser])
				VALUES (@RoleID,@FullName,@UserName,@Email,HASHBYTES('SHA2_512',@Password),GETDATE(),@InsertUser,@InsertUser)
			-- =======================================================

        IF ( @@trancount > 0
                 AND @lLocalTran = 1
               ) 
                BEGIN
                    COMMIT TRANSACTION
                END
        END TRY
        BEGIN CATCH
            IF ( @@trancount > 0
                 AND XACT_STATE() <> 0
               ) 
                BEGIN
                    ROLLBACK TRANSACTION
                END

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END

    SET NOCOUNT OFF
    SET XACT_ABORT OFF
