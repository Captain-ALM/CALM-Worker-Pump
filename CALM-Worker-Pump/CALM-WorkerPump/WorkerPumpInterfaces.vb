''' <summary>
''' This Interface is used to get and set the WorkerPump a Supporting Instance Uses.
''' </summary>
''' <remarks></remarks>
Public Interface IWorkerPumpReceiver
    ''' <summary>
    ''' Gets or sets the WorkerPump of a supporting class.
    ''' </summary>
    ''' <value>The worker pump instance.</value>
    ''' <returns>The worker pump the class uses.</returns>
    ''' <remarks></remarks>
    Property WorkerPump As WorkerPump
End Interface
''' <summary>
''' This Interface is Used For Event Parsers.
''' </summary>
''' <remarks></remarks>
Public Interface IEventParser
    Inherits IWorkerPumpReceiver
    ''' <summary>
    ''' Parses a WorkerEvent.
    ''' </summary>
    ''' <param name="ev">The WorkerEvent to parse.</param>
    ''' <remarks></remarks>
    Sub Parse(ev As WorkerEvent)
End Interface