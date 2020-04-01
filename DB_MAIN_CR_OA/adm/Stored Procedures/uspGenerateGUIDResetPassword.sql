-- ======================================================================
-- Name: [adm].[uspGenerateGUIDResetPassword]
-- Desc: Se utiliza para la creación GUIDs de verificacion para la autorización del cambio de contraseña
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/25/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspGenerateGUIDResetPassword]
	@Email		VARCHAR(50)
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
				DECLARE @UserID		INT
				DECLARE	@FullName	VARCHAR(50)
				DECLARE	@GUID		VARCHAR(MAX)

				SELECT	@UserID		= [UserID],
						@FullName	= [FullName]
				FROM	[adm].[utbUsers]
				WHERE	[Email] = @Email
						AND [ActiveFlag] = 1
				
				IF(@UserID IS NOT NULL)
					BEGIN
						SET @GUID = NEWID();

						INSERT INTO [adm].[utbResetPasswords] ([GUID],[UserID],[InsertUser],[LastModifyUser])
						VALUES (@GUID,@UserID,@FullName,@FullName)

						SELECT	[GUID]		=	@GUID
								,[UserID]	=	@UserID
								,[FullName]	=	@FullName
					END
				ELSE
					BEGIN
						SELECT	[GUID]		=	'XXXX'
								,[UserID]	=	0
								,[FullName]	=	@Email
					END
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
