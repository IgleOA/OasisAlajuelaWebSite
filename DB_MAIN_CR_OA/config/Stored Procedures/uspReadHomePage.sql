-- ======================================================================
-- Name: [config].[uspReadHomePage]
-- Desc: Retorna los detalles principales del HomePage
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadHomePage]
	@ActiveFlag BIT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[HomePageID]
						,[DailyVerse]
						,[DailyVerseReference]
						,[ServicesTitle]
						,[ServicesDescription]
						,[SermonsTitle]
						,[SermonsDescription]
				FROM	[config].[utbHomePage]
				WHERE	[ActiveFlag] = ISNULL(@ActiveFlag,[ActiveFlag])
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