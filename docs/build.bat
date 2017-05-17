SET trg=html
SET tmpfile=doc.md

for %%f in (src\*.md) do type "%%f" >> %tmpfile% && echo. >> %tmpfile%

pandoc -o dist\doc.%trg% %tmpfile% --latex-engine=xelatex -V mainfont="Calibri" --toc --toc-depth=4 -c github-pandoc.css

del %tmpfile%

rem pause