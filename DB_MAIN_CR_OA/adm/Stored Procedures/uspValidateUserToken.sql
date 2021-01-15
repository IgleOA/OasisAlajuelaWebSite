-- ======================================================================
-- Name: [adm].[uspValidateUserToken]
-- Desc: Valida si el Token esta activo
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 08/03/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspValidateUserToken]
	@Token VARCHAR(5000)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[Token], [UserID], [ExpiresDate]
				FROM	[adm].[utbUserTokens]
				WHERE	[Token] = @Token
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