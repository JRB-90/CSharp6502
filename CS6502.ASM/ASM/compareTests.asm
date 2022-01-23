.feature force_range
.debuginfo +
.setcpu "65c02"
.macpack longbranch
.list on

.feature org_per_seg
.segment "ZEROPAGE"
.zeropage
.org $0000

; Main code
.segment "CODE"
.org $8000

Reset:
    
    CLC
    CLD
    CLI
    CLV

    ; Immediate
    LDA #%00000000
    CMP #%00000000
    LDX #%00000000
    CPX #%00000000
    LDY #%00000000
    CPY #%00000000

    LDA #%10000000
    CMP #%00000000
    LDX #%10000000
    CPX #%00000000
    LDY #%10000000
    CPY #%00000000

    LDA #%00000000
    CMP #%10000000
    LDX #%00000000
    CPX #%10000000
    LDY #%00000000
    CPY #%10000000

    LDA #%10000000
    CMP #%10000000
    LDX #%10000000
    CPX #%10000000
    LDY #%10000000
    CPY #%10000000

    LDA #%01000000
    CMP #%00000000
    LDX #%01000000
    CPX #%00000000
    LDY #%01000000
    CPY #%00000000

    LDA #%00000000
    CMP #%01000000
    LDX #%00000000
    CPX #%01000000
    LDY #%00000000
    CPY #%01000000

    ; Zero Page
    LDA #%00000000
    STA $05

    LDA #%00000000
    CMP $05
    LDX #%00000000
    CPX $05
    LDY #%00000000
    CPY $05

    LDA #%10000000
    CMP $05
    LDX #%10000000
    CPX $05
    LDY #%10000000
    CPY $05

    LDA #%10000000
    STA $05

    LDA #%00000000
    CMP $05
    LDX #%00000000
    CPX $05
    LDY #%00000000
    CPY $05

    LDA #%10000000
    CMP $05
    LDX #%10000000
    CPX $05
    LDY #%10000000
    CPY $05

    LDA #%00000000
    STA $05

    LDA #%01000000
    CMP $05
    LDX #%01000000
    CPX $05
    LDY #%01000000
    CPY $05

    LDA #%01000000
    STA $05

    LDA #%00000000
    CMP $05
    LDX #%00000000
    CPX $05
    LDY #%00000000
    CPY $05

    LDA #%00000000
    STA $05

    ; Absolute
    LDA #%00000000
    STA $5005

    LDA #%00000000
    CMP $5005
    LDX #%00000000
    CPX $5005
    LDY #%00000000
    CPY $5005

    LDA #%10000000
    CMP $5005
    LDX #%10000000
    CPX $5005
    LDY #%10000000
    CPY $5005

    LDA #%10000000
    STA $5005

    LDA #%00000000
    CMP $5005
    LDX #%00000000
    CPX $5005
    LDY #%00000000
    CPY $5005

    LDA #%10000000
    CMP $5005
    LDX #%10000000
    CPX $5005
    LDY #%10000000
    CPY $5005

    LDA #%00000000
    STA $5005

    LDA #%01000000
    CMP $5005
    LDX #%01000000
    CPX $5005
    LDY #%01000000
    CPY $5005

    LDA #%01000000
    STA $5005

    LDA #%00000000
    CMP $5005
    LDX #%00000000
    CPX $5005
    LDY #%00000000
    CPY $5005

    LDA #%00000000
    STA $5005

    ; Zero Page, X
    LDX #$03
    LDY #$00
    LDA #%00000000
    STA $08

    LDA #%00000000
    CMP $05,X
    LDA #%10000000
    CMP $05,X

    LDA #%10000000
    STA $08

    LDA #%00000000
    CMP $05,X
    LDA #%10000000
    CMP $05,X

    LDA #%00000000
    STA $08,X

    LDA #%01000000
    CMP $05,X

    LDA #%01000000
    STA $08

    LDA #%00000000
    CMP $05,X

    LDA #%00000000
    STA $08

    ; Absolute, X
    LDX #$03
    LDY #$00
    LDA #%00000000
    STA $5008

    LDA #%00000000
    CMP $5005,X
    LDA #%10000000
    CMP $5005,X

    LDA #%10000000
    STA $5008

    LDA #%00000000
    CMP $5005,X
    LDA #%10000000
    CMP $5005,X

    LDA #%00000000
    STA $5008

    LDA #%01000000
    CMP $5005,X

    LDA #%01000000
    STA $5008

    LDA #%00000000
    CMP $5005,X

    LDA #%00000000
    STA $5008

    ; Absolute, Y
    LDX #$00
    LDY #$03
    LDA #%00000000
    STA $5008

    LDA #%00000000
    CMP $5005,Y
    LDA #%10000000
    CMP $5005,Y

    LDA #%10000000
    STA $5008

    LDA #%00000000
    CMP $5005,Y
    LDA #%10000000
    CMP $5005,Y

    LDA #%00000000
    STA $5008

    LDA #%01000000
    CMP $5005,Y

    LDA #%01000000
    STA $5008

    LDA #%00000000
    CMP $5005,Y

    LDA #%00000000
    STA $5008

    ; Indirect,X
    LDY #$00
    LDA #$05
    STA $08
    LDA #$50
    STA $09
    LDX #$03

    LDA #%00000000
    STA $5005
    
    LDA #%00000000
    CMP ($05,X)
    LDA #%10000000
    CMP ($05,X)

    LDA #%10000000
    CMP ($05,X)

    LDA #%10000000
    STA $5005

    LDA #%00000000
    CMP ($05,X)

    LDA #%10000000
    CMP ($05,X)

    LDA #%00000000
    STA $5005

    LDA #%01000000
    CMP ($05,X)

    LDA #%01000000
    STA $5005

    LDA #%00000000
    CMP ($05,X)

    LDA #%00000000
    STA $5005

    ; Indirect,Y
    LDX #$00
    LDA #$05
    STA $05
    LDA #$50
    STA $06
    LDY #$03

    LDA #%00000000
    STA $5008
    
    LDA #%00000000
    CMP ($05,X)
    LDA #%10000000
    CMP ($05,X)

    LDA #%10000000
    CMP ($05,X)

    LDA #%10000000
    STA $5008

    LDA #%00000000
    CMP ($05,X)

    LDA #%10000000
    CMP ($05,X)

    LDA #%00000000
    STA $5008

    LDA #%01000000
    CMP ($05,X)

    LDA #%01000000
    STA $5008

    LDA #%00000000
    CMP ($05,X)

    LDA #%00000000
    STA $5008

    BRK

IRQ:
    RTI

NMI:
    RTI

; Interupt vectors
.segment "VECTORS"
.org $FFFA
	.word	NMI
	.word	Reset
	.word	IRQ
