# dostools

A collection of .NET 2 command-line utilities written in C#.

Distributed under the MIT license.

My other open source projects can be found [here](http://www.vurdalakov.net/opensource).

## datetime

Prints date and/or time in specified format.

##### Syntax

```
datetime [/options]
```

`/format` option sets the date and/or time output format. See [here](http://msdn.microsoft.com/ru-ru/library/zdtaw1bw.aspx) for details.

`/adddays` option adds the specified number of days to the current date. The number of days can be negative or positive.

`/newline` option adds a new line (CRLF) after output.

##### Examples

```dos
datetime
datetime /format:"dd.MM.yyyy HH.mm.ss.ffff"
datetime /adddays:-1 /format:yyyyMMdd
datetime /newline
```

Usage in .BAT files:

```
for /f "usebackq" %%i in (`datetime.exe /adddays:-1 /format:yyyy-MM-dd`) do set date=%%i
echo %date%
```

## xslt

Command-line XSLT processor.

##### Syntax

```
xslt <data file> <style sheet file> <output file> [/options] ...
```

`/EnableScript` option enables support for embedded script blocks in XSLT style sheets.

##### Examples

```
xslt userdata.xml template.xsl userdata.html /EnableScript
```

## sleep

Delays for a specified amount of time.

##### Syntax

```
sleep <number>[suffix] [/options]
```

Pauses for `number` of seconds, minutes, hours or days.

`number` can be integer or floating point number.

`suffix` can be `s` for seconds (default), `m` for minutes, `h` for hours or `d` for days.

`/showdelay` option prints total tool run time.

##### Examples

```
sleep 10
sleep 10s
sleep 60m
sleep 24h
sleep  7d

sleep 3.7
sleep 3.7e1

sleep 10s /showdelay
sleep 11.5m
sleep 0.1h -showdelay
```
