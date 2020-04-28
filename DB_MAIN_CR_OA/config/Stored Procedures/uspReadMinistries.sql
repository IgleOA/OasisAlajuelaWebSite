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

CREATE PROCEDURE [config].[uspReadMinistries]
	@MinistryID INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[MinistryID]
						,[Name]
						,[Description]
						,[Image]
						,[ImageExt]
						,[ActionLink]
						,[ActiveFlag]						
				FROM	[config].[utbMinistries]
				WHERE	[ActiveFlag]  = 1
						AND [MinistryID] = ISNULL(@MinistryID,[MinistryID])
				ORDER BY [Name]
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
