# VR Pac-Man Testing Guidelines

## Overview

This document provides guidelines for writing and running unit tests for the VR Pac-Man project. The goal is to ensure code quality, reliability, and maintainability throughout the development process.

## Testing Framework

The project uses Unity's built-in Test Framework (com.unity.test-framework) for unit testing. Tests are organized into two categories:

1. **Edit Mode Tests** - Run in the Unity Editor without entering Play Mode
2. **Play Mode Tests** - Run in a built player or in Play Mode in the Editor

## Test Structure

### Folder Organization
```
Assets/
├── Tests/
│   ├── EditMode/
│   │   ├── GameSystem/
│   │   ├── AI/
│   │   ├── Player/
│   │   ├── UI/
│   │   └── Utils/
│   └── PlayMode/
│       └── Integration/
```

### Naming Conventions
- Test classes should be named after the class they test with "Tests" suffix (e.g., `GameManagerTests`)
- Test methods should follow the pattern: `MethodName_StateUnderTest_ExpectedBehavior`
- Example: `AddScore_PositiveValue_IncreasesScore`

## Writing Tests

### Basic Test Structure
```csharp
[Test]
public void MethodName_StateUnderTest_ExpectedBehavior()
{
    // Arrange - Set up test conditions
    var system = new SystemUnderTest();
    
    // Act - Execute the method under test
    var result = system.MethodToTest();
    
    // Assert - Verify the expected outcome
    Assert.AreEqual(expectedValue, result);
}
```

### Test Setup and Teardown
```csharp
[SetUp]
public void SetUp()
{
    // Code to run before each test
}

[TearDown]
public void TearDown()
{
    // Code to run after each test (cleanup)
}
```

### Common Assertions
- `Assert.AreEqual(expected, actual)` - Check equality
- `Assert.IsTrue(condition)` - Check boolean condition
- `Assert.IsNotNull(object)` - Check object is not null
- `Assert.Throws<T>(code)` - Check exception is thrown

## Test Categories

### Unit Tests
Focus on testing individual components in isolation:
- GameManager functionality
- Ghost AI behaviors
- Player controller mechanics
- Utility functions
- Data structures and algorithms

### Integration Tests
Test how multiple components work together:
- Player-ghost interactions
- Game state transitions
- UI system integration
- Audio-visual feedback systems

### Performance Tests
Verify performance requirements:
- Frame rate stability (72/90/120Hz target)
- Memory usage limits
- Load times
- Battery consumption

## Test Coverage Goals

### Core Systems (100% coverage)
- GameManager state management
- Score calculation and tracking
- Game progression logic

### AI Systems (90% coverage)
- Ghost behavior patterns
- State transition logic
- Pathfinding algorithms

### Player Systems (90% coverage)
- Movement controls
- Interaction mechanics
- Input handling

### Utility Systems (100% coverage)
- Extension methods
- Helper functions
- Data processing

## Running Tests

### In Unity Editor
1. Open the Test Runner window (`Window > General > Test Runner`)
2. Select Edit Mode or Play Mode tab
3. Click "Run All" or select specific tests to run

### Command Line
```bash
# Run all edit mode tests
unity-editor -runTests -testPlatform editmode -projectPath /path/to/project

# Run all play mode tests
unity-editor -runTests -testPlatform playmode -projectPath /path/to/project
```

## Continuous Integration

All tests should pass before merging code to the main branch. The CI pipeline will:
1. Build the project
2. Run all edit mode tests
3. Run all play mode tests
4. Generate test coverage report
5. Block merge if tests fail

## Best Practices

### Test Design
- Keep tests independent and isolated
- Use descriptive test names
- Follow AAA pattern (Arrange, Act, Assert)
- Test one behavior per test method
- Avoid testing implementation details

### Mocking and Dependencies
- Use dependency injection to make components testable
- Mock external services and systems
- Avoid testing Unity engine internals
- Use test doubles for complex dependencies

### Performance Considerations
- Keep tests fast and lightweight
- Avoid frame-dependent logic in edit mode tests
- Use `yield return null` for frame waits in play mode tests
- Minimize object creation in tests

## Adding New Tests

1. Create a new test class in the appropriate folder
2. Follow naming conventions
3. Write tests for new functionality
4. Ensure all tests pass before committing
5. Update documentation if needed

## Troubleshooting

### Common Issues
- **Tests not discovered**: Check test class and method naming
- **Null reference exceptions**: Ensure proper setup in [SetUp] method
- **Test order dependencies**: Make tests independent
- **Timeout issues**: Optimize test performance

### Debugging Tests
- Use `Debug.Log` statements for debugging
- Set breakpoints in test code
- Use Unity's Profiler to identify performance issues
- Check Unity console for error messages

## Maintenance

Regular test maintenance includes:
- Updating tests when functionality changes
- Removing obsolete tests
- Adding new tests for new features
- Refactoring tests for better organization
- Monitoring test execution times

## Resources

- [Unity Test Framework Documentation](https://docs.unity3d.com/Packages/com.unity.test-framework@latest)
- [NUnit Documentation](https://nunit.org/)
- [Unity Testing Best Practices](https://unity.com/how-to/testing-best-practices)