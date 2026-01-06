def merge_arrays(A, B):
    m, n = len(A), len(B)
    C = []
    i = j = 0
    while i < m or j < n:
        if i >= m:  
            C.append(B[j])
            j += 1
        elif j >= n:  
            C.append(A[i])
            i += 1
        else:  
            if A[i] <= B[j]:
                C.append(A[i])
                i += 1
            else:
                C.append(B[j])
                j += 1
    return C

def smallest_common(A, B, C):
    m, n, p = len(A), len(B), len(C)
    i = j = k = 0
    while i < m and j < n and k < p:
        if A[i] == B[j] == C[k]:  
            return A[i]
        
        min_val = min(A[i], B[j], C[k])
        if A[i] == min_val:
            i += 1
        if B[j] == min_val:
            j += 1
        if C[k] == min_val:
            k += 1
    return None  

def search_in_matrix(A, x):
    m, n = len(A), len(A[0])
    i, j = 0, n - 1 
    while i < m and j >= 0:
        if A[i][j] == x:
            return True
        elif A[i][j] > x:
            j -= 1  
        else:
            i += 1  
    return False

def can_obtain_B_from_A(A, B):
    m, n = len(A), len(B)
    i = j = 0
    while i < m and j < n:
        if A[i] == B[j]:
            i += 1
            j += 1
        elif A[i] < B[j]:
            i += 1  
        else:
            return False  
    return j == n  

if __name__ == "__main__":
    # Завдання 1
    A1 = [1, 2, 3, 4]
    B1 = [2, 3, 5]
    print("Завдання 1:", merge_arrays(A1, B1))  

    # Завдання 2
    A2 = [1, 2, 3, 4]
    B2 = [2, 3, 4, 5]
    C2 = [0, 2, 4, 6]
    print("Завдання 2:", smallest_common(A2, B2, C2))  

    # Завдання 3
    A3 = [
        [1, 3, 5],
        [2, 4, 6],
        [3, 5, 7]
    ]
    x = 4
    print("Завдання 3:", search_in_matrix(A3, x))  

    # Завдання 4
    A4 = [1, 2, 3, 4]
    B4 = [2, 4]
    print("Завдання 4:", can_obtain_B_from_A(A4, B4))  

