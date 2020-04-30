-- ======================================================================
-- Name: [adm].[uspReadWebDirectory]
-- Desc: Retorna el WebDirectory
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspReadWebDirectory]
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[WebID]
						,[AppID]
						,[Controller]
						,[Action]
						,[PublicMenu]
						,[AdminMenu]
						,[DisplayName]
						,[Parameter]
						,[Order]						
				FROM	[adm].[utbWebDirectory]
				WHERE	[ActiveFlag] = 1
				ORDER BY [Order]
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