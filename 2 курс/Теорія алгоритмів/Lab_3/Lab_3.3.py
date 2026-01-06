import unittest

def counting_sort_by_digit(arr, exp):
    n = len(arr)
    output = [0] * n
    count = [0] * 10  

    for num in arr:
        index = (num // exp) % 10
        count[index] += 1

    for i in range(1, 10):
        count[i] += count[i - 1]

    for num in reversed(arr):
        index = (num // exp) % 10
        output[count[index] - 1] = num
        count[index] -= 1

    for i in range(n):
        arr[i] = output[i]

def radix_sort(arr):
    if not arr:
        return arr

    max_num = max(arr)  
    exp = 1  

    while max_num // exp > 0:
        counting_sort_by_digit(arr, exp)
        exp *= 10  

    return arr

arr = [170, 451, 755, 900, 802, 241, 256, 669]
print("Не посортована послідовність:", arr)
sorted_arr = radix_sort(arr)
print("Посортована послідовність:", sorted_arr)


class TestSortingAlgorithms(unittest.TestCase):

    def test_radix_sort(self):
        # Звичайний набір чисел
        self.assertEqual(radix_sort([170, 45, 75, 90, 802, 24, 2, 66]), [2, 24, 45, 66, 75, 90, 170, 802])
        self.assertEqual(radix_sort([5, 1, 4, 2, 8]), [1, 2, 4, 5, 8])
        
        # Випадок з однаковими числами
        self.assertEqual(radix_sort([3, 3, 3]), [3, 3, 3])
        
        # Порожній масив
        self.assertEqual(radix_sort([]), [])
        
        # Один елемент
        self.assertEqual(radix_sort([1]), [1])
        
        # Випадок з від’ємними числами (Radix Sort не підтримує від’ємні)
        # self.assertEqual(radix_sort([-5, -1, -10, 0, 5]), [-10, -5, -1, 0, 5])  # Radix Sort не підтримує негативні числа

        # Випадок, де числа мають однакові початкові цифри
        self.assertEqual(radix_sort([123, 124, 122, 121]), [121, 122, 123, 124])
        
        # Випадок із вже відсортованим масивом
        self.assertEqual(radix_sort([1, 2, 3, 4, 5]), [1, 2, 3, 4, 5])
        
        # Великий набір чисел
        self.assertEqual(radix_sort([1000, 500, 100, 10, 1]), [1, 10, 100, 500, 1000])

if __name__ == "__main__":
    unittest.main()