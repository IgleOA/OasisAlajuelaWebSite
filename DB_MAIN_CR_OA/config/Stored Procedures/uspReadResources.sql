-- ======================================================================
-- Name: [config].[uspReadResources]
-- Desc: Retorna los detalles de los recursos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadResources]
	@ResourceTypeID INT = NULL,
	@ActiveFlag BIT = NULL,
	@ResourceID	INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	S.[ResourceID]
						,S.[ResourceTypeID]
						,M.[TypeName] 		
						,S.[FileType]
						,S.[FileData]
						,S.[FileExt]
						,S.[FileName]
						,S.[FileURL]
						,S.[Description]
						,S.[ActiveFlag]
				FROM	[config].[utbResources] S
						LEFT JOIN [config].[utbResourceTypes] M ON M.[ResourceTypeID] = S.[ResourceTypeID]
				WHERE	S.[ActiveFlag] = ISNULL(@ActiveFlag,S.[ActiveFlag])
						AND S.ResourceTypeID = ISNULL(@ResourceTypeID,S.[ResourceTypeID])
						AND S.[ResourceID] = ISNULL(@ResourceID,S.[ResourceID])
				ORDER BY S.[InsertDate] DESC
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
