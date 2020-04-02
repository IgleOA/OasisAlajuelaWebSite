﻿-- ======================================================================
-- Name: [config].[uspReadUpcommingEvents]
-- Desc: Retorna los eventos proximos
-- Auth: Jonathan Piedra johmstone@gmail.com
-- Date: 3/13/2020
-------------------------------------------------------------
-- Change History
-------------------------------------------------------------
-- CI	Date		Author		Description
-- --	----		------		-----------------------------
-- ======================================================================

CREATE PROCEDURE [config].[uspReadUpcommingEvents]
	@pDate DATETIME,
	@pUpCommingFlag BIT,
	@pActiveFlag BIT = NULL,
	@pEventID INT = NULL
AS 
    BEGIN
        SET NOCOUNT ON
        BEGIN TRY
            DECLARE @lErrorMessage NVARCHAR(4000)
            DECLARE @lErrorSeverity INT
            DECLARE @lErrorState INT

            -- =======================================================
				IF(@pUpCommingFlag = 1)
					BEGIN
						SET LANGUAGE Spanish
						SELECT	IE.[EventID]
								,IE.[Title]
								,IE.[MinisterID]
								,[MinisterName]		= M.[Title] + ' ' + M.[FullName]
								,IE.[Description]
								,IE.[ScheduledDate]
								,[ScheduledTime]	= CONVERT(TIME,	IE.[ScheduledDate])
								,IE.[ActiveFlag]
								,[Month]			= DATENAME(MONTH,[ScheduledDate])
								,[Day]				= CASE WHEN DATEPART(DAY,[ScheduledDate]) <10 THEN '0' + CONVERT(VARCHAR(1),DATEPART(DAY,[ScheduledDate]))
														   ELSE CONVERT(VARCHAR(2),DATEPART(DAY,[ScheduledDate])) END
								,[Time]				= CONVERT(VARCHAR(10),FORMAT(IE.[ScheduledDate], 'hh:mm tt','en-US'))
						FROM	[config].[utbUpcomingEvents] IE
								LEFT JOIN [config].[utbMinisters] M ON M.[MinisterID] = IE.[MinisterID]
						WHERE	IE.[ActiveFlag] = ISNULL(@pActiveFlag,IE.[ActiveFlag])
								AND IE.[EventID] = ISNULL(@pEventID,[EventID])
								AND IE.[ScheduledDate] >= @pDate
					END
				ELSE
					BEGIN
						DECLARE @StartDate			DATE	=	@pDate
								,@NumberOfMonths	INT		=	2
								,@Time				TIME	=	CONVERT(TIME,@pDate)

						SET DATEFIRST 7
						SET DATEFORMAT mdy
						SET LANGUAGE US_ENGLISH

						DECLARE @CutoffDate DATE	=	DATEADD(MONTH, @NumberOfMonths, @StartDate)


						SELECT	[EventID]			= 1
								,[Title]			= W.[WorshipName]
								,[MinisterID]		= 1
								,[MinisterName]		= NULL
								,[Description]		= W.[Description]
								,[ScheduledDate]	= CONVERT(DATETIME,Y.[Date]) + CONVERT(DATETIME,W.[Schedule])
								,[ScheduledTime]	= W.[Schedule]
								,[ActiveFlag]		= 1
								,[Month]			= DATENAME(MONTH,CONVERT(DATETIME,Y.[Date]) + CONVERT(DATETIME,W.[Schedule]))
								,[Day]				= CONVERT(VARCHAR(2),DATEPART(DAY,CONVERT(DATETIME,Y.[Date]) + CONVERT(DATETIME,W.[Schedule])))
								,[Time]				= CONVERT(VARCHAR(10),FORMAT((CONVERT(DATETIME,Y.[Date]) + CONVERT(DATETIME,W.[Schedule])), 'hh:mm tt','en-US'))
						FROM	(SELECT [Date] = DATEADD(DAY, rn - 1, @StartDate)
								 FROM (	SELECT	TOP	
												(DATEDIFF(DAY, @StartDate, @CutoffDate)) 
												rn = ROW_NUMBER() OVER (ORDER BY s1.[object_id])
										FROM	sys.all_objects S1
												CROSS JOIN sys.all_objects S2
										ORDER BY s1.[object_id]) X) Y
								LEFT JOIN [config].[utbWorships] W ON W.[Weekday] = DATEPART(WEEKDAY,[Date]) AND W.[ActiveFlag] = 1
						WHERE	DATEPART(WEEKDAY,[Date]) IN (1,5)
								AND CONVERT(DATETIME,Y.[Date]) + CONVERT(DATETIME,W.[Schedule]) >= @pDate
						UNION 
						SELECT	IE.[EventID]
								,IE.[Title]
								,IE.[MinisterID]
								,[MinisterName]		= M.[Title] + ' ' + M.[FullName]
								,IE.[Description]
								,IE.[ScheduledDate]
								,[ScheduledTime]	= CONVERT(TIME,	IE.[ScheduledDate])
								,IE.[ActiveFlag]
								,[Month]			= DATENAME(MONTH,[ScheduledDate])
								,[Day]				= CASE WHEN DATEPART(DAY,[ScheduledDate]) <10 THEN '0' + CONVERT(VARCHAR(1),DATEPART(DAY,[ScheduledDate]))
														   ELSE CONVERT(VARCHAR(2),DATEPART(DAY,[ScheduledDate])) END
								,[Time]				= CONVERT(VARCHAR(10),FORMAT(IE.[ScheduledDate], 'hh:mm tt','en-US'))
						FROM	[config].[utbUpcomingEvents] IE
								LEFT JOIN [config].[utbMinisters] M ON M.[MinisterID] = IE.[MinisterID]
						WHERE	IE.[ActiveFlag] = 1
								AND IE.[ScheduledDate] >= @pDate

						ORDER BY [ScheduledDate]
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
	SELECT FORMAT(GETDATE(), 'hh:mm tt','en-US')