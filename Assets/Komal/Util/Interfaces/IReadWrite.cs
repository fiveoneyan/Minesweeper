/* Brief: NativeMessageBoxManager
 * Author: Komal
 * Date: "2019-07-10"
 */

namespace komal {
    interface IReadJsonFile<T> {
        T ReadJsonFile(string filePath);
    }

    interface IWriteJsonFile<T> {
        void WriteJsonFile(T data);
    }

    interface IReadWriteFile<T> : IReadJsonFile<T>, IWriteJsonFile<T>
    { 
    }
}
