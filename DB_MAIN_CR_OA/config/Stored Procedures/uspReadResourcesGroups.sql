-- ======================================================================
-- Name: [config].[uspReadResourcesGroups]
-- Desc: Retorna los detalles de recursos por grupo
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadResourcesGroups]
	@GroupID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	RG.[ResourceGroupID]
						,RG.[GroupID]
						,RG.[ResourceTypeID]
						,RT.[TypeName]
				FROM	[config].[utbResourcesGroups] RG
						LEFT JOIN [config].[utbResourceTypes] RT ON RT.[ResourceTypeID] = RG.ResourceTypeID
				WHERE	RG.[ActiveFlag] = 1
						AND RG.[GroupID] = @GroupID
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