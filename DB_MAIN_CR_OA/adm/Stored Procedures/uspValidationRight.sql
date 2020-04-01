-- ======================================================================
-- Name: [adm].[uspValidationRight]
-- Desc: Valida los derechos de cada usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspValidationRight]
	@UserName	VARCHAR(50),
	@Controller VARCHAR(50),
	@Action		VARCHAR(50)
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	W.[WebID]
						,[ReadRight]	= CONVERT(BIT,ISNULL(R.[Read],0))
						,[WriteRight]	= CONVERT(BIT,ISNULL(R.[Write],0))
			
			  FROM		[adm].[utbWebDirectory] W
						OUTER APPLY (SELECT	RR.[Read]
											,RR.[Write]
									 FROM	[adm].[utbRightsbyRole] RR
											INNER JOIN [adm].[utbUsers] U ON U.[RoleID] = RR.[RoleID] 
									 WHERE  RR.[WebID] = W.[WebID]
											AND U.[UserName] = @UserName) R
			  WHERE		W.[Controller] = @Controller
						AND W.[Action] = @Action											
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
	/*
	
	*/