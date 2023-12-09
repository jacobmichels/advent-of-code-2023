package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
	"unicode"
)

type Grid struct {
	grid [][]rune
}

func (g *Grid) Print() {
	for i, row := range g.grid {
		for j := range row {
			fmt.Printf("%c", g.grid[i][j])
		}
		fmt.Println()
	}
}

func (g *Grid) GetSum() int {
	sum := 0

	for _, row := range g.grid {
		sum += SumRow(row)
	}

	return sum
}

func SumRow(row []rune) int {
	sum := 0

	tmp := strings.Builder{}
	for i := 0; i < len(row); i++ {
		c := row[i]
		fmt.Printf("rune: %c", c)
		if unicode.IsDigit(c) {
			tmp.WriteRune(c)
		} else {
			// we convert tmp to an int, add it to sum, and clear tmp
			if tmp.Len() == 0 {
				continue
			}
			num, err := strconv.Atoi(tmp.String())
			if err != nil {
				panic(err)
			}
			sum += num
			tmp.Reset()
		}
	}

	if tmp.Len() != 0 {
		num, err := strconv.Atoi(tmp.String())
		if err != nil {
			panic(err)
		}
		sum += num
	}

	fmt.Printf("row sum: %d\n", sum)

	return sum
}

func (g *Grid) Wipe(row, col int) {
	// we check in here if the indexes given are valid
	if row < 0 || row >= len(g.grid) || col < 0 || col >= len(g.grid[0]) {
		return
	}

	c := g.grid[row][col]
	if !unicode.IsDigit(c) {
		return
	}

	g.grid[row][col] = '.'

	g.Wipe(row, col+1)
	g.Wipe(row, col-1)
}

func (g *Grid) RemovePartNumbers() {
	// this should use recursion
	for i, row := range g.grid {
		for j, c := range row {
			if !unicode.IsDigit(c) && c != '.' {
				g.Wipe(i+1, j+1)
				g.Wipe(i+1, j)
				g.Wipe(i+1, j-1)
				g.Wipe(i, j+1)
				g.Wipe(i, j-1)
				g.Wipe(i-1, j+1)
				g.Wipe(i-1, j)
				g.Wipe(i-1, j-1)
			}
		}
	}
}

func main() {
	grid := parseGrid()

	// I could sum all numbers, then mutate the grid removing actual part numbers (setting them to .), then summing the grid again and taking the difference between the two numbers

	sumOfAllNumbers := grid.GetSum()
	grid.Print()
	grid.RemovePartNumbers()
	fmt.Println("---------------")
	grid.Print()
	invalidPartNumbersSum := grid.GetSum()

	fmt.Printf("%d, %d\n", sumOfAllNumbers, invalidPartNumbersSum)

	realSum := sumOfAllNumbers - invalidPartNumbersSum
	fmt.Println(realSum)
}

func parseGrid() *Grid {
	lines := getLines()
	grid := make([][]rune, 0, len(lines))

	for _, line := range lines {
		entry := make([]rune, 0, len(line))
		for _, c := range line {
			if c == '\n' {
				continue
			}
			entry = append(entry, c)
		}
		grid = append(grid, entry)
	}

	return &Grid{grid}
}

func getLines() []string {
	scanner := bufio.NewScanner(os.Stdin)
	var lines []string

	for scanner.Scan() {
		lines = append(lines, scanner.Text())
	}

	return lines
}
