using System;

namespace Redbox.KioskEngine.ComponentModel
{
  public interface IQueueService : IDisposable
  {
    ErrorList Clear();

    ErrorList GetDepth(out int count);

    ErrorList Enqueue(byte priority, string type, object message);

    ErrorList EnqueueKioskEvent(object message);

    ErrorList EnqueueOnlyOne(byte priority, string type, object message, out int newMessageId);

    ErrorList Update(int id, string message);

    ErrorList ExportToXml(string fileName);

    ErrorList Initialize(string path, IQueueServiceData queueServiceData);

    ErrorList ForEach(Action<IMessage> action);

    ErrorList ForEach(Action<IMessage> action, string type);

    bool IsWorkerStarted();

    void StopQueueWorker();

    void StartQueueWorker();

    ErrorList DeleteMessage(int id);

    ErrorList DeleteMessageByType(string type);

    ErrorList Dequeue(Predicate<IMessage> predicate);

    int QueueReadPeriod { get; }

    T GetObjectFromMessageData<T>(IMessage message);

    void ForEachPriority(Action<IQueueServicePriority> action);

    bool RegisterMessageProcessorService(
      IQueueMessageProcessorService queueMessageProcessorService);

    bool UnRegisterMessageProcessorService(
      IQueueMessageProcessorService queueMessageProcessorService);
  }
}
