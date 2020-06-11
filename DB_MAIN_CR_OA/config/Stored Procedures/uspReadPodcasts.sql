-- ======================================================================
-- Name: [config].[uspReadPodcasts]
-- Desc: Retorna los detalles de los Podcasts
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadPodcasts]
	@PodcastID INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				SET LANGUAGE Spanish
				SELECT	P.[PodcastID]
						,P.[Title]
						,P.[Description]
						,P.[BannerData]
						,P.[BannerExt]
						,P.[MinisterID]
						,[MinisterName] = M.[Title] + ' ' + M.[FullName]
						,P.[ActiveFlag]
						,[Date]			= CONVERT(DATE,ISNULL(P.[LastModifyDate],P.[InsertDate]))
						,[Year]			= CONVERT(VARCHAR(4),YEAR(ISNULL(P.[LastModifyDate],P.[InsertDate])))
						,[Month]		= DATENAME(MONTH,ISNULL(P.[LastModifyDate],P.[InsertDate]))
						,[Day]			= CASE WHEN DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate])) <10 THEN '0' + CONVERT(VARCHAR(1),DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate])))
											   ELSE CONVERT(VARCHAR(2),DATEPART(DAY,ISNULL(P.[LastModifyDate],P.[InsertDate]))) END
						,[Slide]		= ROW_NUMBER() OVER(ORDER BY P.[LastModifyUser] DESC, P.[InsertDate] DESC) - 1
				FROM	[config].[utbPodcasts] P
						LEFT JOIN [config].[utbMinisters] M ON M.[MinisterID] = P.[MinisterID]
				WHERE	P.[ActiveFlag] = 1
						AND P.[PodcastID] = ISNULL(@PodcastID,P.[PodcastID])
				ORDER BY P.[LastModifyUser] DESC, P.[InsertDate] DESC
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