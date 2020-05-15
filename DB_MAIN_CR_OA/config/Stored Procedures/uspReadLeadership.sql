-- ======================================================================
-- Name: [config].[uspReadLeadership]
-- Desc: Retorna los lideres de la base de datos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadLeadership]
	@LeaderID INT = NULL	
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[LeaderID]
						,[FullName]
						,[Description]
						,[Image]
						,[ImageExt]
						,[ActionLink]
						,[Order]						
				FROM	[config].[utbLeadership]
				WHERE	[ActiveFlag]  = 1
						AND [LeaderID] = ISNULL(@LeaderID,[LeaderID])
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
