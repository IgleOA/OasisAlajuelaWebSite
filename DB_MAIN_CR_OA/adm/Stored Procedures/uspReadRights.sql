-- ======================================================================
-- Name: [adm].[uspReadRights]
-- Desc: Retorna los derechos de cada rol
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspReadRights]
	@RoleID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	W.[WebID]
						,[RoleID]		= @RoleID
						,W.[DisplayName]
						,[RightID]		= ISNULL(RR.[RightID],0)
						,[ReadRight]	= CONVERT(BIT,ISNULL(RR.[Read],0))
						,[WriteRight]	= CONVERT(BIT,ISNULL(RR.[Write],0))
				FROM	[adm].[utbWebDirectory] W
						LEFT JOIN [adm].[utbRightsbyRole] RR ON RR.[WebID] = W.[WebID] 
																AND RR.[ActiveFlag] = 1
																AND RR.[RoleID] = @RoleID
				WHERE	W.[ActiveFlag] = 1
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