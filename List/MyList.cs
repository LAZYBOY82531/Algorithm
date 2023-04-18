using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List
{
    internal class MyList<T>
    {
        private const int DCapacity = 8;           //기본 list의 최대크기
        private T[] items;                               //리스트
        private int size;                                  //원소가 들어가 있는 장소갯수 (Index)

        public MyList()
        {
            this.items = new T[DCapacity];
            this.size = 0;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= size)     //예외처리
                    throw new ArgumentOutOfRangeException();
                return items[index];                 //리스트에서 불러올 값
            }
            set
            {
                if (index < 0 || index >= size)    //예외처리
                    throw new ArgumentOutOfRangeException();
                items[index] = value;                //리스트에 저장할 값
            }
        }
        public int Count()                           //리스트에 원소가 들어가 있는 갯수를 반환하는 함수
        {
            return size + 1;
        }
        public int Capacity()                      //리스트의 실질적 값을 반환하는 함수
        {
            return items.Length;
        }
        public void Clear()                       //리스트 초기화하는 함수
        {
            T[] newItems = new T[DCapacity];
            items = newItems;
            size = 0;
        }

        public void Add(T item)               //리스트에 값넣는 함수
        {
            if (size < items.Length)           //값을 넣을 Index가 배열의 크기보다 큰지 계산
            {
                items[size++] = item;         //Index에 값넣고 덮어쓰지 않기위해 size값 키움
            }
            else                                      //배열의 크기보다 size가 큰 경우 오류가 나기때문에 grow함수 적용
            {
                Grow();                             //배열을 늘리는 함수
                items[size++] = item;
            }
        }
        public bool Remove(T item)          //리스트에서 값빼고 뺏으면 true 값이 없어서 못빼면 false를 내는 bool함수
        {
            int index = IndexOf(item);      //item값이 리스트에 있는지 확인
            if (index >= 0)                         //있는 경우
            {
                RemoveAt(index);               //리스트에서 item제거
                return true;                        //true값 반환
            }
            else
            {
                //못 찾은 경우
                return false;
            }
        }
        public void RemoveAt(int index)                                      //위치에 있는 값 제거하는 함수
        {
            if (index < 0 || index >= size)                                       //예외처리
                throw new IndexOutOfRangeException();
            size--;
            Array.Copy(items, index + 1, items, index, size - index);       //리스트에서 값 제거하고 새로 만드는 함수
        }

        public int IndexOf(T item)                                     //리스트에 item값이 있는지 확인하고 그 Index값을 반환하는 함수
        {
            return Array.IndexOf(items, item, 0, size);       
        }

        public T? Find(Predicate<T> match)                          //리스트에 조건에 맞는 값 반환하는 함수
        {
            if (match == null)                                                  //예외처리
                throw new ArgumentNullException("match");

            for (int i = 0; i < size; i++)                                    //맨 앞부터 찾음
            {
                if (match(items[i]))
                    return items[i];
            }
            return default(T);                                                //없으면 기본값 반환
        }
        public T? FindLast(Predicate<T> match)                       //리스트에 조건에 맞는 값 뒤에서부터 찾는 함수
        {
            if (match == null)                                                      //예외처리
                throw new ArgumentNullException("match");

            for (int i = size; i < 0; i--)                                         //뒤에부터 찾음
            {
                if (match(items[i]))
                    return items[i];
            }
            return default(T);                                                    //없으면 기본값 반환
        }

        public int FindIndex(Predicate<T> match)                    //조건에 맞는 값의 위치 반환하는 함수
        {
            for (int i = 0; i < size; i++)
            {
                if (match(items[i]))
                    return i;
            }
            return -1;                                                                 //없으면 -1반환
        }
        public int FindLastIndex(Predicate<T> match)             //조건에 맞는 값중 맨 뒤에있는 값의 위치 반환하는 함수
        {
            for (int i = size; i < 0; i--)
            {
                if (match(items[i]))
                    return i;
            }
            return -1;                                                                //맞는 조건이 없으면 -1반환
        }
        private void Grow()                                                    // 더 큰 배열을 만든 후 그 전에 있던 값을 복사하는 함수
        {
            int newCapacity = items.Length * 2;
            T[] newItems = new T[newCapacity];
            Array.Copy(items, 0, newItems, 0, size);
            items = newItems;
        }
    }
    /*배열 : 동일한 자료형들이 <색인, 원소>의 순서쌍으로 집단화한 선형자료구조
     * 하나의 변수에 여러 값을 저장하는 데 쓰이는 정적 리스트 구조. 원소의 갯수가 정해져서 항상 마지막 원소가 존재함.
     * Index를 이용해서 자료형이 같은 데이터를 관리 및 집합 내에서 상대적인 위치의 식별이 가능함.
     * 순차적 접근 > 주소계산이 쉬움. 임의접근 > 주소만 있으면 일정 시간 내 접근 가능
     * 시스템 데이터형으로 주로 연속 기억공간 할당으로 구현되며 기억공간 할당 방식과는 독립적
     * 정보의 은닉 >  Index를 가지고 어떻게 원소 값에 접근하느냐는 사용자는 몰라도됨.
     * 
     * 선형 리스트 : 자료를 구조화하는 가장 기본적인 방법으로 자료를 나열한 목록 또는 자료간 순서를 갖는 리스트
     * 자료들이 순서대로 연속적으로 메모리에 저장됨. 원소들의 논리적 순서와 같은 순서로 기억공간에 저장하고 원소들의 논리적순서 = 원소가 저장된 물리적 순서임.
     * 삽입, 삭제 시 순서에 변함이 없슴
     * 접근속도가 빠람
     * 알고리즘이 간단함.*/
}
