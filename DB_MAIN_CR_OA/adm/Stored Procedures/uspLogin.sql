-- ======================================================================
-- Name: [adm].[uspLogin]
-- Desc: Se utiliza para la validación de los usuarios al logearse
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 03/25/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspLogin]
	@TxtName	VARCHAR(50),
	@Password	VARCHAR(50)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				DECLARE @UserID	INT,
						@UserName VARCHAR(50)

				IF EXISTS(SELECT * FROM [adm].[utbUsers] WHERE [UserName] = @TxtName OR [Email] = @TxtName)
					BEGIN
						SELECT	@UserID = [UserID]
								,@UserName = [UserName]
						FROM	[adm].[utbUsers] 
						WHERE	([UserName] = @TxtName 
								 OR [Email] = @TxtName)
								AND [PasswordHash] = HASHBYTES('SHA2_512',@Password)
								AND [ActiveFlag] = 1
						
						SELECT	[UserID] = ISNULL(@UserID,-1),
								[UserName] = @UserName
					END
				ELSE
					BEGIN
						SELECT	[UserID] = 0 /*Usuario No registrado*/
								,[UserName] = @UserName
					END	
			-- =======================================================

        END TRY
        BEGIN CATCH

            SELECT  @lErrorMessage = ERROR_MESSAGE() ,
                    @lErrorSeverity = ERROR_SEVERITY() ,
                    @lErrorState = ERROR_STATE()       

            RAISERROR (@lErrorMessage, @lErrorSeverity, @lErrorState);
        END CATCH
    END
    SET NOCOUNT OFF