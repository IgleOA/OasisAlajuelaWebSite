-- ======================================================================
-- Name: [adm].[uspReadUsersProfile]
-- Desc: Retorna el perfil de un usuario
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [adm].[uspReadUsersProfile]
	@UserID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	U.[UserID]
						,U.[RoleID]
						,U.[FullName]
						,U.[UserName]
						,U.[Email]
						,U.[ActiveFlag]
						,[LastActivityDate]	= ISNULL(U.[LastActivityDate],U.[CreationDate])
						,U.[CreationDate]
						,[RoleName]			= CASE WHEN R.[RoleName] = 'Nuevo Usuario' THEN 'Usuario' ELSE R.[RoleName] END
						,UP.[PhotoPath]
						,UP.[Phone]
						,UP.[Mobile]
						,UP.[Facebook]
						,UP.[Twitter]
						,UP.[Snapchat]
						,UP.[Instragram]
						,UP.[Country]
						,UP.[State]
						,UP.[City]
				FROM	[adm].[utbUsers] U
						LEFT JOIN [adm].[utbRoles] R ON R.RoleID = U.[RoleID]
						LEFT JOIN [adm].[utbUsersProfile] UP ON UP.[UserID] = U.[UserID]
				WHERE	U.[UserID] = @UserID
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