using MappingGenerator.LangObjects;
using System.Collections.Generic;
using System.Collections;

namespace MappingGenerator
{
    public class ClassFiles : IList<ClassFile>
    {
        private List<ClassFile> _classFiles = new List<ClassFile>();

        private int _currentClassFileIndex = 0;

        private int _maxClassDefinitionsPerFile;

        private List<ClassDefinition> _trackedClassDefinitions = new List<ClassDefinition>();

        public ClassFiles() : this(1) {}

        public ClassFiles(int maxClassDefinitionsPerFile)
        {
            _maxClassDefinitionsPerFile = maxClassDefinitionsPerFile;
        }

        public ClassFile this[int index]
        {
            get { return _classFiles[index]; }
            set { _classFiles[index] = value; }
        }

        public int Count
        {
            get { return _classFiles.Count; }
        }

        public bool IsReadOnly { get { return false; } }

        public void Add(ClassFile item)
        {
            _classFiles.Add(item);
        }

        public void Add(ClassDefinition classDefinition)
        {
            var classFile = CurrentClassFile();
            if (!_trackedClassDefinitions.Contains(classDefinition))
            {
                _trackedClassDefinitions.Add(classDefinition);
                classFile.Classes.Add(classDefinition);
            }
        }

        public void Clear()
        {
            _classFiles.Clear();
            _currentClassFileIndex = 0;
            _trackedClassDefinitions.Clear();
        }

        public bool Contains(ClassFile item)
        {
            return _classFiles.Contains(item);
        }

        public void CopyTo(ClassFile[] array, int arrayIndex)
        {
            _classFiles.CopyTo(array, arrayIndex);
        }

        public IEnumerator<ClassFile> GetEnumerator()
        {
            return _classFiles.GetEnumerator();
        }

        public int IndexOf(ClassFile item)
        {
            return _classFiles.IndexOf(item);
        }

        public void Insert(int index, ClassFile item)
        {
            _classFiles.Insert(index, item);
        }

        public bool Remove(ClassFile item)
        {
            return _classFiles.Remove(item);
        }

        public void RemoveAt(int index)
        {
            _classFiles.RemoveAt(index);
        }

        private ClassFile CurrentClassFile()
        {
            InitCurrentClassFile();

            if (_classFiles[_currentClassFileIndex].Classes.Count >= _maxClassDefinitionsPerFile)
                MoveNextClassFile();

            return _classFiles[_currentClassFileIndex];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _classFiles.GetEnumerator();
        }

        private void InitCurrentClassFile()
        {
            if (_currentClassFileIndex >= _classFiles.Count)
                _classFiles.Add(new ClassFile());

            if (_classFiles[_currentClassFileIndex].Classes == null)
                _classFiles[_currentClassFileIndex].Classes = new List<ClassDefinition>();
        }

        private void MoveNextClassFile()
        {
            _currentClassFileIndex++;
            InitCurrentClassFile();
        }
    }
}
