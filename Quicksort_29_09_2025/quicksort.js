function quicksort(arr) {
    if (arr.length <= 1) {
        return arr;
    }
    const pivot = arr[arr.length - 1];
    const left = [];
    const right = [];
    for (let i = 0; i < arr.length - 1; i++) {
        if (arr[i] < pivot) {
            left.push(arr[i]);
        } else {
            right.push(arr[i]);
        }
    }
    return [...quicksort(left), pivot, ...quicksort(right)];
}

const array = [3, 6, 8, 10, 1, 2, 1];
console.log("Unsorted array:", array);
const sortedArray = quicksort(array);
console.log("Sorted array:", sortedArray);