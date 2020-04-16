-- ======================================================================
-- Name: [config].[uspReadResourceTypes]
-- Desc: Retorna los detalles de los tipos de recursos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadResourceTypes]
	@ActiveFlag		BIT = NULL,
	@ResourceTypeID	INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	[ResourceTypeID]
						,[TypeName] 
						,[TypeImage]
						,[TypeImageExt]
						,[Description]
						,[ActiveFlag]
				FROM	[config].[utbResourceTypes]
				WHERE	[ActiveFlag] = ISNULL(@ActiveFlag,[ActiveFlag])
						AND [ResourceTypeID] = ISNULL(@ResourceTypeID,[ResourceTypeID])
				ORDER BY [InsertDate]
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
