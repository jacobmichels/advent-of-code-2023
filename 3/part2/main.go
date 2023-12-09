package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"unicode"
)

type Grid struct {
	grid [][]rune
}

type Coordinates struct {
	row int
	col int
}

func (g *Grid) Wipe(row, col int) string {
	// we check in here if the indexes given are valid
	if row < 0 || row >= len(g.grid) || col < 0 || col >= len(g.grid[0]) {
		return ""
	}

	c := g.grid[row][col]
	if !unicode.IsDigit(c) {
		return ""
	}

	g.grid[row][col] = '.'

	suffix := g.Wipe(row, col+1)
	prefix := g.Wipe(row, col-1)

	return fmt.Sprintf("%s%c%s", prefix, c, suffix)
}

func (g *Grid) ListPossibleGearLocations() []Coordinates {
	var coordinates []Coordinates
	for i, row := range g.grid {
		for j, c := range row {
			if c == '*' {
				coordinates = append(coordinates, Coordinates{row: i, col: j})
			}
		}
	}

	return coordinates
}

func (g *Grid) FindGearRatios(possibleGears []Coordinates) []int {
	var ratios []int
	for _, gear := range possibleGears {
		i := gear.row
		j := gear.col

		var partNumbersRaw []string

		partNumbersRaw = append(partNumbersRaw, g.Wipe(i+1, j+1))
		partNumbersRaw = append(partNumbersRaw, g.Wipe(i+1, j))
		partNumbersRaw = append(partNumbersRaw, g.Wipe(i+1, j-1))
		partNumbersRaw = append(partNumbersRaw, g.Wipe(i, j+1))
		partNumbersRaw = append(partNumbersRaw, g.Wipe(i, j-1))
		partNumbersRaw = append(partNumbersRaw, g.Wipe(i-1, j+1))
		partNumbersRaw = append(partNumbersRaw, g.Wipe(i-1, j))
		partNumbersRaw = append(partNumbersRaw, g.Wipe(i-1, j-1))

		var partNumbers []string
		for _, raw := range partNumbersRaw {
			if raw != "" {
				partNumbers = append(partNumbers, raw)
			}
		}

		if len(partNumbers) == 2 {
			// convert the part numbers to integers, multiply them, append to results slice
			one, err := strconv.Atoi(partNumbers[0])
			if err != nil {
				panic(err)
			}
			two, err := strconv.Atoi(partNumbers[1])
			if err != nil {
				panic(err)
			}
			ratios = append(ratios, one*two)
		}
	}

	return ratios
}

func main() {
	grid := parseGrid()

	possibleGears := grid.ListPossibleGearLocations()
	ratios := grid.FindGearRatios(possibleGears)

	sum := 0
	for _, ratio := range ratios {
		sum += ratio
	}
	fmt.Println(sum)
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
