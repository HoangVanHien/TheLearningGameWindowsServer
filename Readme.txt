Unity version 2021.3.24f1
Windows 10

Tạo một project rỗng, đặt Assets trong git vào thay thế Assets trong project rỗng

Đặt các file .dll từ 
https://drive.google.com/drive/folders/1mOsuoVN_B8y9WSyUwFE2IQBtfqIawqvB?usp=sharing
vào Assets/Plugins

Sửa đổi file Assets/Main/GameManager.cs dòng 35
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"E:\C#\Unity\TheLearningGameWindowsServer\TheLearningGameWindowsServer\cloudfire.json");
sửa phần "E:\C#\Unity\TheLearningGameWindowsServer\TheLearningGameWindowsServer\cloudfire.json"
thành đường dẫn tới file the-learning-game-143304-firebase-adminsdk-lrd4p-ea551ee815.json mà em đặt trong Assets/Main

Mở Assets/Main/Scene/SampleScene trong Unity
Chạy, gặp lỗi
RpcException: Status(StatusCode="Internal", Detail="Bad gRPC response. Response protocol downgraded to HTTP/1.1.")
Grpc.Net.Client.Internal.HttpContentClientStreamReader`2[TRequest,TResponse].MoveNextCore (System.Threading.CancellationToken cancellationToken) (at <c521f2934b38415a9aef98ca65fec1a0>:0)
Google.Cloud.Firestore.FirestoreDb.<GetDocumentSnapshotsAsync>b__37_0 (Google.Cloud.Firestore.V1.BatchGetDocumentsRequest req, Google.Api.Gax.Grpc.CallSettings settings) (at <3f61ea659613450da4e85e64f00bc902>:0)
Google.Cloud.Firestore.RetryHelper.Retry[TRequest,TResponse] (System.Func`3[T1,T2,TResult] fn, TRequest request, Google.Api.Gax.Grpc.CallSettings callSettings, Google.Api.Gax.IClock clock, Google.Api.Gax.IScheduler scheduler) (at <3f61ea659613450da4e85e64f00bc902>:0)
Google.Cloud.Firestore.FirestoreDb.GetDocumentSnapshotsAsync (System.Collections.Generic.IEnumerable`1[T] documents, Google.Protobuf.ByteString transactionId, Google.Cloud.Firestore.FieldMask fieldMask, System.Threading.CancellationToken cancellationToken) (at <3f61ea659613450da4e85e64f00bc902>:0)
Google.Cloud.Firestore.DocumentReference.GetSnapshotAsync (Google.Protobuf.ByteString transactionId, System.Threading.CancellationToken cancellationToken) (at <3f61ea659613450da4e85e64f00bc902>:0)
FirebaseDatabaseManager.GetPlayerData (System.String userID, UnityEngine.Events.UnityAction`1[T0] action) (at Assets/Main/Scripts/FirebaseDatabaseManager.cs:86)
System.Runtime.CompilerServices.AsyncMethodBuilderCore+<>c.<ThrowAsync>b__7_0 (System.Object state) (at <4a4789deb75f446a81a24a1a00bdd3f9>:0)
UnityEngine.UnitySynchronizationContext+WorkRequest.Invoke () (at <f712b1dc50b4468388b9c5f95d0d0eaf>:0)
UnityEngine.UnitySynchronizationContext.Exec () (at <f712b1dc50b4468388b9c5f95d0d0eaf>:0)
UnityEngine.UnitySynchronizationContext.ExecuteTasks () (at <f712b1dc50b4468388b9c5f95d0d0eaf>:0)