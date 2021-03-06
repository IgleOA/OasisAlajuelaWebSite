﻿-- ======================================================================
-- Name: [config].[uspReadUsersGroups]
-- Desc: Retorna los detalles de usuarios por grupo
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadUsersGroups]
	@GroupID INT
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	UG.[UserGroupID]
						,UG.[GroupID]
						,UG.[UserID]
						,U.[FullName]
				FROM	[config].[utbUsersGroups] UG
						LEFT JOIN [adm].[utbUsers] U ON U.[UserID] = UG.[UserID]
				WHERE	UG.[ActiveFlag] = 1
						AND UG.[GroupID] = @GroupID
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