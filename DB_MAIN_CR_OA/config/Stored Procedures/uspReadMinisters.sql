-- ======================================================================
-- Name: [config].[uspReadMinistries]
-- Desc: Retorna los Ministerios de la base de datos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadMinisters]
	@pActiveFlag BIT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[MinisterID]
						,[Title]
						,[FullName]
						,[ActiveFlag]						
				FROM	[config].[utbMinisters]
				WHERE	[ActiveFlag]  = ISNULL(@pActiveFlag,[ActiveFlag])
				ORDER BY [FullName]
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