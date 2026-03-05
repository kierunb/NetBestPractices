# Using ref safety in your application
These techniques are low-level performance tuning. They can increase performance in your application when applied to hot paths, and when you've measured the impact before and after the changes. In most cases, the cycle you'll follow is:

- **Measure allocations**: Determine what types are being allocated the most, and when you can reduce the heap allocations.
- **Convert class to struct**: Many times, types can be converted from a class to a struct. Your app uses stack space instead of making heap allocations.
- **Preserve semantics**: Converting a class to a struct can impact the semantics for parameters and return values. Any method that modifies its parameters should now mark those parameters with the ref modifier. That ensures the modifications are made to the correct object. Similarly, if a property or method return value should be modified by the caller, that return should be marked with the ref modifier.
- **Avoid copies**: When you pass a large struct as a parameter, you can mark the parameter with the in modifier. You can pass a reference in fewer bytes, and ensure that the method doesn't modify the original value. You can also return values by readonly ref to return a reference that can't be modified.


Use ".NET object allocation tool" in release mode.