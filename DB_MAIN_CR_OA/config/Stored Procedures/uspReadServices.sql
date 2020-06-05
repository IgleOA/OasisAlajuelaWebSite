-- ======================================================================
-- Name: [config].[uspReadServices]
-- Desc: Retorna los servicios disponibles
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadServices]
	@pActiveFlag	BIT = NULL,
	@ServiceID		INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[ServiceID]
						,[ServiceName]
						,[ServiceDescription]
						,[ServiceIcon]
						,[Order]
						,[ControllerLink]
						,[ActionLink]
						,[ActiveFlag]						
				FROM	[config].[utbServices]
				WHERE	[ActiveFlag]  = ISNULL(@pActiveFlag,[ActiveFlag])
						AND [ServiceID] = ISNULL(@ServiceID, [ServiceID])
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