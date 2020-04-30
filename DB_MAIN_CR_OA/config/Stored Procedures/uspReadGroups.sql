-- ======================================================================
-- Name: [config].[uspReadGroups]
-- Desc: Retorna los detalles de los grupos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadGroups]
	@GroupID INT = NULL,
	@UserID INT = NULL,
	@ResourceTypeID INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@UserID IS NULL AND @ResourceTypeID IS NULL)
					BEGIN
						SELECT	G.[GroupID]
								,G.[GroupName]
								,G.[Description]
						FROM	[config].[utbGroups] G
						WHERE	G.[ActiveFlag] = 1
								AND G.[GroupID] = ISNULL(@GroupID,G.[GroupID])
						ORDER BY G.[GroupName]
					END
				ELSE
					BEGIN
						IF(@UserID IS NOT NULL)
							BEGIN
								SELECT	DISTINCT
										G.[GroupID]
										,G.[GroupName]
										,G.[Description]    
								FROM	[config].[utbGroups] G
										LEFT JOIN [config].[utbUsersGroups] UG ON UG.[GroupID] = G.[GroupID] AND UG.[ActiveFlag] = 1
								WHERE	G.[ActiveFlag] = 1
										AND UG.[UserID] = @UserID									
								ORDER BY G.[GroupName]
							END
						ELSE
							BEGIN
								SELECT	DISTINCT
										G.[GroupID]
										,G.[GroupName]
										,G.[Description]   						
								FROM	[config].[utbGroups] G
										LEFT JOIN [config].[utbResourcesGroups] RG ON RG.[GroupID] = G.[GroupID] AND RG.[ActiveFlag] = 1
								WHERE	G.[ActiveFlag] = 1
										AND RG.[ResourceTypeID] = @ResourceTypeID
								ORDER BY G.[GroupName]
							END
					END
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