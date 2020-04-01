-- ======================================================================
-- Name: [config].[uspReadSermons]
-- Desc: Retorna los detalles de los sermones
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadSermons]
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SELECT	S.[SermonID]
						,S.[Title]
						,S.[Description]
						,S.[Tags]
						,S.[MinisterID]
						,[MinisterName] =	M.[Title] + ' ' + M.[FullName]
						,S.[SermonDate]
						--,S.[SermonURL]
						,[SermonURL]	=	REPLACE(REPLACE(REPLACE(S.[SermonURL],'https://youtu.be/','https://www.youtube.com/embed/'),'https://www.youtube.com/watch?v=','https://www.youtube.com/embed/'),'\','')
						,S.[BackgroundImage]
						,S.[BackgroundExt]
						,S.[ActiveFlag]
				FROM	[config].[utbSermons] S
						LEFT JOIN [config].[utbMinisters] M ON M.[MinisterID] = S.[MinisterID]
				ORDER BY S.[SermonDate] DESC
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
