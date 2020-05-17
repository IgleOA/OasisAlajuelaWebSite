CREATE FUNCTION [book].[ufnCheckSeatAvailability]
(
	@EventID	INT
	,@SeatID	VARCHAR(10)
	,@Distance	INT
)
RETURNS BIT
AS
BEGIN
	DECLARE @Reserved BIT 
	SELECT	@Reserved	= CASE WHEN	ADS.[Reserved] = 1 THEN 1
							   WHEN C1.[SeatID] IS NOT NULL THEN 1	
							   WHEN C2.[SeatID] IS NOT NULL THEN 1	
							   ELSE 0 END		
		
	FROM	[book].[utbAuditoriumSeats] ADS
			OUTER APPLY (SELECT	TOP 1
								S.[SeatID]
						 FROM	[book].[utbAuditoriumSeats] S
								INNER JOIN [book].[utbReservations] R ON R.[SeatID] = S.[SeatID] 
																		 AND R.[ActiveFlag] = 1
																		 AND R.[EventID] = @EventID
						 WHERE	LEFT(S.[SeatID],2) = LEFT(ADS.[SeatID],2)		-- SAME BLOCK
								AND RIGHT(S.[RowID],1) = RIGHT(ADS.[RowID],1)	--  SAME ROW
								AND ABS(S.[Label] - ADS.[Label]) <= @Distance				
								) C1		
			OUTER APPLY (SELECT	TOP 1
							S.[SeatID]
					 FROM	[book].[utbAuditoriumSeats] S
							INNER JOIN [book].[utbReservations] R ON R.[SeatID] = S.[SeatID] 
																	 AND R.[ActiveFlag] = 1
																	 AND R.[EventID] = @EventID
					 WHERE	LEFT(S.[SeatID],2) = LEFT(ADS.[SeatID],2)		-- SAME BLOCK
							AND ABS(CONVERT(INT,RIGHT(S.[RowID],1)) - CONVERT(INT,RIGHT(ADS.[RowID],1))) = 1 -- NEXT OR PREVIOUS ROW
							AND ABS(S.[Label] - ADS.[Label]) <= (@Distance - 1) 
							) C2

	WHERE	ADS.[SeatID] = @SeatID

	RETURN @Reserved
END
