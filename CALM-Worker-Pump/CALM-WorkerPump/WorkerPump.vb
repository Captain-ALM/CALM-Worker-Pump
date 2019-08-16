Imports System.Threading
Imports System.Windows.Forms
''' <summary>
''' Provides a worker pump so forms can have processing done on a different thread through events and parsers.
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class WorkerPump
    Implements IDisposable
    ''' <summary>
    ''' Raised when an Exception Occurs on the pump thread.
    ''' </summary>
    ''' <param name="ex">The exception that occured.</param>
    ''' <remarks></remarks>
    Public Event OnPumpException(ex As Exception)

    Private formInstanceRegistry As New List(Of Form)
    Private workerQueue As New Queue(Of WorkerEvent)
    Private workerStates As New Dictionary(Of WorkerEvent, Boolean)
    Private pump As Boolean = False
    Private parsers As New List(Of IEventParser)
    Private wThread As Thread = Nothing
    ''' <summary>
    ''' Creates a new instance of worker pump.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        wThread = New Thread(New ThreadStart(AddressOf workerRunner))
        wThread.IsBackground = True
    End Sub
    ''' <summary>
    ''' Whether this object has been disposed.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return disp
        End Get
    End Property
    ''' <summary>
    ''' Whether this object is disposing.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Disposing As Boolean
        Get
            Return disping
        End Get
    End Property
    ''' <summary>
    ''' Whether the pump has event data to process.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property PumpBusy As Boolean
        Get
            Return workerQueue.Count > 0
        End Get
    End Property
    ''' <summary>
    ''' Adds a form to the form registry.
    ''' </summary>
    ''' <param name="f">The form instance.</param>
    ''' <remarks></remarks>
    Public Sub addFormInstance(Of t As {Form, IWorkerPumpReceiver})(f As t)
        f.WorkerPump = Me
        formInstanceRegistry.Add(f)
    End Sub
    ''' <summary>
    ''' Starts the pump.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub startPump()
        pump = True
        wThread.Start()
    End Sub
    ''' <summary>
    ''' Returns whether the pump is pumping events.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Pumping() As Boolean
        Get
            Return pump Or wThread.IsAlive
        End Get
    End Property
    ''' <summary>
    ''' Stops the pump.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function stopPump() As Boolean
        pump = False
        Return Not wThread.IsAlive
    End Function
    ''' <summary>
    ''' Force stops the pump.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub stopPumpForce()
        pump = False
        If wThread.IsAlive Then wThread.Join(5000)
        If wThread.IsAlive Then wThread.Abort()
    End Sub
    ''' <summary>
    ''' Adds an event to the EventQueue of the pump.
    ''' </summary>
    ''' <param name="ev">The Worker Event</param>
    ''' <remarks></remarks>
    Public Sub addEvent(ev As WorkerEvent)
        If Not workerStates.ContainsKey(ev) Then
            workerQueue.Enqueue(ev)
            workerStates.Add(ev, True)
        End If
    End Sub
    ''' <summary>
    ''' Adds an event to the EventQueue of the pump.
    ''' </summary>
    ''' <param name="es">Event Source Object</param>
    ''' <param name="et">Event Type</param>
    ''' <param name="ed">Event Args</param>
    ''' <remarks></remarks>
    Public Sub addEvent(es As Object, et As EventType, ed As EventArgs)
        Dim ev As New WorkerEvent(es, et, ed)
        If Not workerStates.ContainsKey(ev) Then
            workerQueue.Enqueue(ev)
            workerStates.Add(ev, True)
        End If
    End Sub
    ''' <summary>
    ''' Adds an event to the EventQueue of the pump.
    ''' </summary>
    ''' <typeparam name="t">The parent source object type</typeparam>
    ''' <param name="es">Event Source Object</param>
    ''' <param name="sp">Event Source Parent Object</param>
    ''' <param name="et">Event Type</param>
    ''' <param name="ed">Event Args</param>
    ''' <remarks></remarks>
    Public Sub addEvent(Of t)(es As Object, sp As t, et As EventType, ed As EventArgs)
        Dim evpl As New List(Of Object)
        evpl.Add(sp)
        Dim ev As New WorkerEvent(es, evpl, et, ed)
        If Not workerStates.ContainsKey(ev) Then
            workerQueue.Enqueue(ev)
            workerStates.Add(ev, True)
        End If
    End Sub
    ''' <summary>
    ''' Adds an event to the EventQueue of the pump.
    ''' </summary>
    ''' <param name="es">Event Source Object</param>
    ''' <param name="sops">Event Source Parent Object List</param>
    ''' <param name="et">Event Type</param>
    ''' <param name="ed">Event Args</param>
    ''' <remarks></remarks>
    Public Sub addEvent(es As Object, sops As List(Of Object), et As EventType, ed As EventArgs)
        Dim ev As New WorkerEvent(es, sops, et, ed)
        If Not workerStates.ContainsKey(ev) Then
            workerQueue.Enqueue(ev)
            workerStates.Add(ev, True)
        End If
    End Sub
    ''' <summary>
    ''' Adds an event parser to the pump.
    ''' </summary>
    ''' <param name="p">The Parser instance.</param>
    ''' <remarks></remarks>
    Public Sub addParser(p As IEventParser)
        p.WorkerPump = Me
        parsers.Add(p)
    End Sub
    ''' <summary>
    ''' Shows a registered form.
    ''' </summary>
    ''' <typeparam name="t">The form type to show.</typeparam>
    ''' <param name="index">The index of the form's register of its type.</param>
    ''' <param name="owner">The owner to have.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function showForm(Of t As Form)(Optional index As Integer = 0, Optional owner As Form = Nothing) As Boolean
        Dim ci As Integer = 0
        If index < 0 Then Return False
        For Each cf As Form In formInstanceRegistry
            If canCastForm(Of t)(cf) Then
                If ci = index Then
                    If owner IsNot Nothing Then
                        cf.ShowDialog(owner)
                    Else
                        cf.ShowDialog()
                    End If
                    Return True
                Else
                    ci = ci + 1
                End If
            End If
        Next
        Return False
    End Function
    ''' <summary>
    ''' Removes a registered form of the specified type.
    ''' </summary>
    ''' <typeparam name="t">The form type.</typeparam>
    ''' <param name="index">The index of the form's register of its type.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function removeForm(Of t As Form)(Optional index As Integer = 0) As Boolean
        Dim ci As Integer = 0
        If index < 0 Then Return False
        For Each cf As Form In formInstanceRegistry
            If canCastForm(Of t)(cf) Then
                If ci = index Then
                    If cf.Visible Then
                        Return False
                    Else
                        Return formInstanceRegistry.Remove(cf)
                    End If
                Else
                    ci = ci + 1
                End If
            End If
        Next
        Return False
    End Function
    ''' <summary>
    ''' Removes all forms of a specified type from the registry.
    ''' </summary>
    ''' <typeparam name="t">The type of form.</typeparam>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function removeForms(Of t As Form)() As Boolean
        Dim toret As Boolean = True
        Dim cnt As Integer = 0
        For Each cf As Form In formInstanceRegistry
            If canCastForm(Of t)(cf) Then
                If cf.Visible Then
                    toret = toret And False
                Else
                    toret = toret And formInstanceRegistry.Remove(cf)
                End If
                cnt += 1
            End If
        Next
        If cnt < 1 Then toret = False
        Return toret
    End Function
    ''' <summary>
    ''' Removes a registered event parser of the specified type.
    ''' </summary>
    ''' <typeparam name="t">The event parser type.</typeparam>
    ''' <param name="index">The index of the event parser's register of its type.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function removeParser(Of t As IEventParser)(Optional index As Integer = 0) As Boolean
        Dim frm As Form = Nothing
        Dim ci As Integer = 0
        If index < 0 Then Return False
        For Each cf As IEventParser In parsers
            If canCastParser(Of t)(cf) Then
                If ci = index Then
                    Return parsers.Remove(cf)
                End If
            Else
                ci = ci + 1
            End If
        Next
        Return False
    End Function
    ''' <summary>
    ''' Removes all event parsers of a specified type from the registry.
    ''' </summary>
    ''' <typeparam name="t">The type of event parser.</typeparam>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function removeParsers(Of t As IEventParser)() As Boolean
        Dim toret As Boolean = True
        Dim cnt As Integer = 0
        For Each cf As IEventParser In parsers
            If canCastParser(Of t)(cf) Then
                toret = toret And parsers.Remove(cf)
                cnt += 1
            End If
        Next
        If cnt < 1 Then toret = False
        Return toret
    End Function

    Private Function canCastForm(Of t As Form)(f As Form) As Boolean
        Try
            Dim nf As t = f
            Return True
        Catch ex As InvalidCastException
            Return False
        End Try
    End Function

    Private Function canCastParser(Of t As IEventParser)(f As IEventParser) As Boolean
        Try
            Dim nf As t = f
            Return True
        Catch ex As InvalidCastException
            Return False
        End Try
    End Function

    Private Function castObject(Of t)(f As Object) As t
        Try
            Dim nf As t = f
            Return nf
        Catch ex As InvalidCastException
            Return Nothing
        End Try
    End Function

    Private Function canCastObject(Of t)(f As Object) As Boolean
        Try
            Dim nf As t = f
            Return True
        Catch ex As InvalidCastException
            Return False
        End Try
    End Function

    Private Sub workerRunner()
        Try
            While pump
                Try
                    While workerQueue.Count > 0
                        Dim ev As WorkerEvent = workerQueue.Dequeue()
                        parseEvents(ev)
                        If workerStates.ContainsKey(ev) Then
                            workerStates.Remove(ev)
                        End If
                        Thread.Sleep(125)
                    End While
                Catch ex As ThreadAbortException
                    Throw ex
                Catch ex As Exception
                    RaiseEvent OnPumpException(ex)
                End Try
                Thread.Sleep(125)
            End While
        Catch ex As ThreadAbortException
            Throw ex
        Catch ex As Exception
            RaiseEvent OnPumpException(ex)
        End Try
        pump = False
    End Sub

    Sub parseEvents(ev As WorkerEvent)
        For Each parser As IEventParser In parsers
            parser.Parse(ev)
        Next
    End Sub

#Region "IDisposable Support"
    Private disposedValue As Boolean = False ' To detect redundant calls
    Private disping As Boolean = False
    Private disp As Boolean = False

    ' IDisposable
    Private Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                Me.disping = True
                workerQueue.Clear()
                workerStates.Clear()
                For Each frm As Form In formInstanceRegistry
                    If Not frm.IsDisposed And Not frm.Disposing Then
                        frm.Dispose()
                    End If
                Next
                formInstanceRegistry.Clear()
                For Each par As IEventParser In parsers
                    If canCastObject(Of IDisposable)(par) Then
                        Dim disppar As IDisposable = castObject(Of IDisposable)(par)
                        disppar.Dispose()
                    End If
                Next
                parsers.Clear()
            End If

            ' t.o.d.o. free unmanaged resources (unmanaged objects) and override Finalize() below.
            wThread = Nothing
            pump = Nothing
            workerQueue = Nothing
            workerStates = Nothing
            formInstanceRegistry = Nothing
            parsers = Nothing
            Me.disping = False
            Me.disp = True
        End If
        Me.disposedValue = True
    End Sub

    't.o.d.o. override Finalize() only if Dispose(ByVal disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    ''' <summary>
    ''' Releases all the resources of the contained objects.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        If Pumping() Then Throw New InvalidOperationException("Stop the workerpump before disposing.")
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
