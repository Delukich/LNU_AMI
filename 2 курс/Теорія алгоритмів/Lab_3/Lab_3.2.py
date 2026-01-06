import unittest

def counting_sort(arr):
    if not arr:  
        return []

    k = max(arr)

    count = [0] * (k + 1)

    for num in arr:
        count[num] += 1

    for i in range(1, k + 1):
        count[i] += count[i - 1]

    output = [0] * len(arr)
    for num in reversed(arr):  
        output[count[num] - 1] = num
        count[num] -= 1

    return output

arr = [5, 1, 2, 9, 4, 3, 2]
sorted_arr = counting_sort(arr)
print("Не посортована послідовність:", arr)
print("Посортована послідовність:", sorted_arr)


class TestSortingAlgorithms(unittest.TestCase):
    def test_counting_sort(self):
        # Звичайний випадок
        self.assertEqual(counting_sort([4, 2, 2, 8, 3, 3, 1]), [1, 2, 2, 3, 3, 4, 8])
        self.assertEqual(counting_sort([5, 1, 4, 2, 8]), [1, 2, 4, 5, 8])
        
        # Випадок з однаковими елементами
        self.assertEqual(counting_sort([3, 3, 3]), [3, 3, 3])
        
        # Порожній масив
        self.assertEqual(counting_sort([]), [])
        
        # Один елемент
        self.assertEqual(counting_sort([1]), [1])
        
        # Випадок з вже відсортованим масивом
        self.assertEqual(counting_sort([1, 2, 3, 4, 5]), [1, 2, 3, 4, 5])
        
        # Випадок із повторюваними значеннями
        self.assertEqual(counting_sort([10, 5, 3, 3, 10, 2, 1]), [1, 2, 3, 3, 5, 10, 10])
        
        # Випадок, де всі числа однакові
        self.assertEqual(counting_sort([7, 7, 7, 7]), [7, 7, 7, 7])


if __name__ == "__main__":
    unittest.main()
 


